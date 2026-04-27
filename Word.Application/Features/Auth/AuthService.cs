using Word.Application.Abstractions.Persistence;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.Auth;
using Word.Domain.Entities;

namespace Word.Application.Features.Auth;

public class AuthService : IAuthService
{
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
        ValidateEmailPasswordRequest(request.Email, request.Password);

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Имя обязательно");

        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        var passwordHash = _passwordHasherService.HashPassword(request.Password);

        if (existingUser is not null)
        {
            if (!string.IsNullOrWhiteSpace(existingUser.PasswordHash))
                throw new InvalidOperationException("Пользователь с таким email уже существует");

            existingUser.UpdateProfile(request.Name, request.Email);
            existingUser.SetPasswordHash(passwordHash);
            existingUser.MarkLogin();

            await _userRepository.UpdateAsync(existingUser, cancellationToken);

            return CreateAuthResponse(existingUser);
        }

        var user = new AppUser(
            request.Name,
            request.Email,
            null,
            passwordHash);

        await _userRepository.AddAsync(user, cancellationToken);

        return CreateAuthResponse(user);
    }

    public async Task<AuthResponseDto> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ValidateEmailPasswordRequest(request.Email, request.Password);

        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || string.IsNullOrWhiteSpace(user.PasswordHash))
            throw new UnauthorizedAccessException("Неверный email или пароль");

        var isValidPassword = _passwordHasherService.VerifyPassword(request.Password, user.PasswordHash);

        if (!isValidPassword)
            throw new UnauthorizedAccessException("Неверный email или пароль");

        user.MarkLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);

        return CreateAuthResponse(user);
    }

    public async Task<AuthResponseDto> SignInWithGoogleAsync(
        string idToken,
        CancellationToken cancellationToken = default)
    {
        var googleUser = await _googleTokenVerifier.VerifyAsync(idToken, cancellationToken);

        var user = await _userRepository.GetByGoogleIdAsync(googleUser.GoogleId, cancellationToken);

        if (user is null)
        {
            user = await _userRepository.GetByEmailAsync(googleUser.Email, cancellationToken);

            if (user is null)
            {
                user = new AppUser(
                    googleUser.Name,
                    googleUser.Email,
                    googleUser.GoogleId,
                    null);

                await _userRepository.AddAsync(user, cancellationToken);
            }
            else
            {
                user.UpdateProfile(googleUser.Name, googleUser.Email);
                user.SetGoogleId(googleUser.GoogleId);
                user.MarkLogin();

                await _userRepository.UpdateAsync(user, cancellationToken);
            }
        }
        else
        {
            user.UpdateProfile(googleUser.Name, googleUser.Email);
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

    private static void ValidateEmailPasswordRequest(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email обязателен");

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Пароль обязателен");

        if (password.Length < 6)
            throw new ArgumentException("Пароль должен быть не меньше 6 символов");
    }
}
