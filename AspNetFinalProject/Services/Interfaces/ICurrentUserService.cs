using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Services.Interfaces;

public interface ICurrentUserService
{
    Task<UserProfile?> GetUserProfileAsync();
    string? GetIdentityId();
    Task<bool> UpdateAsync(UpdateUserProfileDto updateDto);
    Task<bool> HasWorkspaceRoleAsync(Guid workspaceId, params ParticipantRole[] roles);
    Task<bool> HasBoardRoleAsync(Guid boardId, params ParticipantRole[] roles);
    Task<ParticipantRole?> GetBoardRoleAsync(Guid boardId);
    Task<ParticipantRole?> GetWorkSpaceRoleAsync(Guid boardId);
}