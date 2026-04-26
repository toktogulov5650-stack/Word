namespace Word.API.Contracts.Auth;

public class CurrentUserResponse
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
