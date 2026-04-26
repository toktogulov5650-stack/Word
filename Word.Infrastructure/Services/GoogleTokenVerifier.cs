using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.Auth;
using Word.Infrastructure.Configurations;


namespace Word.Infrastructure.Services;

public class GoogleTokenVerifier : IGoogleTokenVerifier
{
    private readonly GoogleAuthOptions _options;

    public GoogleTokenVerifier(IOptions<GoogleAuthOptions> options)
    {
        _options = options.Value;
    }


    public async Task<GoogleUserInfoDto> VerifyAsync(string idToken, CancellationToken cancellationToken = default)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(
            idToken,
            new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _options.ClientId }
            });

        return new GoogleUserInfoDto
        {
            GoogleId = payload.Subject,
            Email = payload.Email,
            Name = payload.Name ?? payload.Email,
        };
    }
}
