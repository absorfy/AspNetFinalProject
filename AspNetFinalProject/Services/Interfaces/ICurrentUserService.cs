using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface ICurrentUserService
{
    Task<UserProfile?> GetUserProfileAsync();
    string? GetIdentityId();
    Task<bool> UpdateAsync(UpdateUserProfileDto updateDto);
}