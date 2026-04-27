using Microsoft.EntityFrameworkCore;
using Word.Application.Abstractions.Persistence;
using Word.Domain.Entities;
using Word.Infrastructure.Persistence;

namespace Word.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<AppUser?> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Users
            .FirstOrDefaultAsync(x => x.GoogleId == googleId, cancellationToken);
    }

    public async Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return await _appDbContext.Users
            .FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);
    }

    public async Task<AppUser?> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }

    public async Task AddAsync(AppUser appUser, CancellationToken cancellationToken = default)
    {
        await _appDbContext.Users.AddAsync(appUser, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AppUser appUser, CancellationToken cancellationToken = default)
    {
        _appDbContext.Users.Update(appUser);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
