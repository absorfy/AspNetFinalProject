using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IWorkSpaceParticipantRepository
{
    Task<IEnumerable<UserProfile>> GetNonParticipantsByUserNameAsync(Guid workSpaceId,
        string userName);
    Task<IEnumerable<UserProfile>> GetNonParticipantsByEmailAsync(Guid workSpaceId,
        string email);
    Task<IEnumerable<WorkSpaceParticipant>> GetByWorkSpaceIdAsync(Guid workSpaceId);
    Task AddAsync(WorkSpaceParticipant participant);
    Task RemoveAsync(Guid workspaceId, string participantId);
    Task SaveChangesAsync();
    Task<bool> IsAlreadyParticipant(Guid workspaceId, string userId);
}