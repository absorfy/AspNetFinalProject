using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IWorkSpaceParticipantRepository
{
    Task AddAsync(WorkSpaceParticipant participant);
    Task SaveChangesAsync();
}