using AspNetFinalProject.Common;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IWorkSpaceParticipantRepository
{
    Task<IEnumerable<UserProfile>> GetNonParticipantsAsync(Guid workSpaceId,
        string search);

    Task<PagedResult<WorkSpaceParticipant>> GetByWorkSpaceIdAsync(Guid workSpaceId, PagedRequest request);
    Task<IEnumerable<WorkSpaceParticipant>> GetByWorkSpaceIdAsync(Guid workSpaceId);
    Task<WorkSpaceParticipant?> GetAsync(Guid workSpaceId, string userProfileId);
    Task AddAsync(WorkSpaceParticipant participant);
    Task RemoveAsync(Guid workspaceId, string participantId);
    Task SaveChangesAsync();
    Task<bool> IsAlreadyParticipant(Guid workspaceId, string userId);
}