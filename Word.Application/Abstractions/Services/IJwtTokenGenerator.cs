


using Word.Domain.Entities;

namespace Word.Application.Abstractions.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser appUser);
}
