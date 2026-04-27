using Word.Application.DTOs.Auth;


namespace Word.Application.Abstractions.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);

    Task<AuthResponseDto> SignInWithGoogleAsync(string idToken, CancellationToken cancellationToken = default);

    Task<CurrentUserDto?> GetCurrentUserAsync(int userId, CancellationToken cancellationToken = default);
}
