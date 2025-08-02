using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface ICurrentUserService
{
    Task<UserProfile?> GetUserProfileAsync();
    string? GetIdentityId();
}