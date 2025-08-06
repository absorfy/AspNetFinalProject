using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IBoardParticipantRepository
{
    Task AddAsync(BoardParticipant participant);
    Task SaveChangesAsync();
}