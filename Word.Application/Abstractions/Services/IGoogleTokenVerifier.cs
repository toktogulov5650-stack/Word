using Word.Application.DTOs.Auth;


namespace Word.Application.Abstractions.Services;

public interface IGoogleTokenVerifier
{
    Task<GoogleUserInfoDto> VerifyAsync(string idToken, CancellationToken cancellationToken = default);
}
