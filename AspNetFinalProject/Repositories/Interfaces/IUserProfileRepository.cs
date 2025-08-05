using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByIdentityId(string identityId);
    Task AddAsync(UserProfile userProfile);
    Task SaveChangesAsync();
}