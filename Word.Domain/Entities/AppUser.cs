

namespace Word.Domain.Entities;

public class AppUser
{
    private AppUser()
    {

    }


    public AppUser(string googleId, string email, string name)
    {
        GoogleId = googleId;
        Email = email;
        CreatedAtUtc = DateTime.UtcNow;
        LastLoginAtUtc = DateTime.UtcNow;
    }


    public int Id { get; private set; }
    public string GoogleId { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime LastLoginAtUtc { get; private set; }


    public void UpdateProfile(string email, string name)
    {
        Email = email;
        Name = name;
        LastLoginAtUtc = LastLoginAtUtc;
    }

}