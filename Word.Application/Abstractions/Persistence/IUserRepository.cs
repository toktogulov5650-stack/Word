using Word.Domain.Entities;


namespace Word.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<AppUser?> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken = default);

    Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<AppUser?> GetByIdAsync(int userId, CancellationToken cancellationToken = default);

    Task AddAsync(AppUser appUser, CancellationToken cancellationToken = default);

    Task UpdateAsync(AppUser appUser, CancellationToken cancellationToken = default);
}
