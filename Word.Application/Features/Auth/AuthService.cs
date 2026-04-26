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


    public AuthService(
        IUserRepository userRepository,
        IGoogleTokenVerifier googleTokenVerifier,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _googleTokenVerifier = googleTokenVerifier;
        _jwtTokenGenerator = jwtTokenGenerator;
    }


    public async Task<AuthResponseDto> SignInWithGoogleAsync(
        string idToken,
        CancellationToken cancellationToken = default)
    {
        var googleUser = await _googleTokenVerifier.VerifyAsync(idToken, cancellationToken);

        var user = await _userRepository.GetByGoogleIdAsync(googleUser.GoogleId, cancellationToken);

        if (user is null)
        {
            user = new AppUser(
                googleUser.GoogleId,
                googleUser.Email,
                googleUser.Name);

            await _userRepository.AddAsync(user, cancellationToken);
        }
        else
        {
            user.UpdateProfile(
                googleUser.Email,
                googleUser.Name);

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

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


    public async Task<CurrentUserDto?> GetCurrentUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            return null;

        return new CurrentUserDto
        {
            Id =user.Id,
            Email = user.Email,
            Name = user.Name,
        };
    }
}
