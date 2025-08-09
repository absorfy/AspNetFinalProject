using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IWorkSpaceParticipantRepository
{
    Task<IEnumerable<WorkSpaceParticipant>> GetByWorkSpaceIdAsync(Guid workSpaceId);
    Task AddAsync(WorkSpaceParticipant participant);
    Task SaveChangesAsync();
}