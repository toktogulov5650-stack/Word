

namespace Word.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public CurrentUserDto User { get; set; } = new();

}
