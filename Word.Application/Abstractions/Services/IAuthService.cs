using Word.Application.DTOs.Auth;


namespace Word.Application.Abstractions.Services;

public interface IAuthService
{
    Task<AuthResponseDto> SignInWithGoogleAsync(string idToken, CancellationToken cancellationToken = default);

    Task<CurrentUserDto?> GetCurrentUserAsync(int userId, CancellationToken cancellationToken = default);
}
