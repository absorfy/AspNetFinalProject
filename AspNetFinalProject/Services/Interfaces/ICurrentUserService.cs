using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Services.Interfaces;

public interface ICurrentUserService
{
    Task<UserProfile?> GetUserProfileAsync();
    string? GetIdentityId();
    Task<bool> UpdateAsync(UpdateUserProfileDto updateDto);
    Task<bool> HasRoleAsync(Guid workspaceId, params WorkSpaceRole[] roles);
}