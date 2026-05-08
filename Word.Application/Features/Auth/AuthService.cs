using Word.Application.Abstractions.Persistence;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.Auth;
using Word.Domain.Entities;

namespace Word.Application.Features.Auth;

public class AuthService : IAuthService
{
    private const int MinimumPasswordLength = 6;

    private readonly IUserRepository _userRepository;
    private readonly IGoogleTokenVerifier _googleTokenVerifier;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasherService _passwordHasherService;

    public AuthService(
        IUserRepository userRepository,
        IGoogleTokenVerifier googleTokenVerifier,
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordHasherService passwordHasherService)
    {
        _userRepository = userRepository;
        _googleTokenVerifier = googleTokenVerifier;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<AuthResponseDto> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var name = ValidateName(request.Name);
        var email = ValidateEmail(request.Email);
        var password = ValidatePassword(request.Password);

        var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);
        var passwordHash = _passwordHasherService.HashPassword(password);

        if (existingUser is not null)
        {
            if (!string.IsNullOrWhiteSpace(existingUser.PasswordHash))
                throw new InvalidOperationException("A user with this email already exists.");

            existingUser.UpdateProfile(name, email);
            existingUser.SetPasswordHash(passwordHash);
            existingUser.MarkLogin();

            await _userRepository.UpdateAsync(existingUser, cancellationToken);

            return CreateAuthResponse(existingUser);
        }

        var user = new AppUser(
            name,
            email,
            null,
            passwordHash);

        await _userRepository.AddAsync(user, cancellationToken);

        return CreateAuthResponse(user);
    }

    public async Task<AuthResponseDto> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var email = ValidateEmail(request.Email);
        var password = ValidatePassword(request.Password);

        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null || string.IsNullOrWhiteSpace(user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var isValidPassword = _passwordHasherService.VerifyPassword(password, user.PasswordHash);

        if (!isValidPassword)
            throw new UnauthorizedAccessException("Invalid email or password.");

        user.MarkLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);

        return CreateAuthResponse(user);
    }

    public async Task<AuthResponseDto> SignInWithGoogleAsync(
        string idToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(idToken))
            throw new ArgumentException("Google ID token is required.", nameof(idToken));

        var googleUser = await _googleTokenVerifier.VerifyAsync(idToken.Trim(), cancellationToken);

        if (string.IsNullOrWhiteSpace(googleUser.Email))
            throw new InvalidOperationException("Google account did not provide an email address.");

        var normalizedName = string.IsNullOrWhiteSpace(googleUser.Name)
            ? googleUser.Email
            : googleUser.Name.Trim();

        var user = await _userRepository.GetByGoogleIdAsync(googleUser.GoogleId, cancellationToken);

        if (user is null)
        {
            user = await _userRepository.GetByEmailAsync(googleUser.Email, cancellationToken);

            if (user is null)
            {
                user = new AppUser(
                    normalizedName,
                    googleUser.Email,
                    googleUser.GoogleId,
                    null);

                await _userRepository.AddAsync(user, cancellationToken);
            }
            else
            {
                user.UpdateProfile(normalizedName, googleUser.Email);
                user.SetGoogleId(googleUser.GoogleId);
                user.MarkLogin();

                await _userRepository.UpdateAsync(user, cancellationToken);
            }
        }
        else
        {
            user.UpdateProfile(normalizedName, googleUser.Email);
            user.MarkLogin();

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        return CreateAuthResponse(user);
    }

    public async Task<CurrentUserDto?> GetCurrentUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            return null;

        return new CurrentUserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name
        };
    }

    private AuthResponseDto CreateAuthResponse(AppUser user)
    {
        var accessToken = _jwtTokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            User = new CurrentUserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            }
        };
    }

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        return name.Trim();
    }

    private static string ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        return email.Trim();
    }

    private static string ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required.", nameof(password));

        var normalizedPassword = password.Trim();

        if (normalizedPassword.Length < MinimumPasswordLength)
            throw new ArgumentException($"Password must be at least {MinimumPasswordLength} characters long.", nameof(password));

        return normalizedPassword;
    }
}
