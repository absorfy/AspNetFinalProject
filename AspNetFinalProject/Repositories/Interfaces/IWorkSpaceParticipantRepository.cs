using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IWorkSpaceParticipantRepository
{
    Task<IEnumerable<WorkSpaceParticipant>> GetByWorkSpaceIdAsync(int workSpaceId);
    Task AddAsync(WorkSpaceParticipant participant);
    Task SaveChangesAsync();
}