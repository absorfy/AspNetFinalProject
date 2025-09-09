using AspNetFinalProject.Common;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IBoardParticipantRepository
{
    Task AddAsync(BoardParticipant participant);
    Task SaveChangesAsync();
    Task<IEnumerable<UserProfile>> GetNonParticipantsAsync(Guid boardId,
        string search);

    Task<BoardParticipant?> GetAsync(Guid boardId, string userProfileId);
    Task<PagedResult<BoardParticipant>> GetByBoardIdAsync(Guid boardId, PagedRequest request);
    Task RemoveAsync(Guid boardId, string participantId);
    Task<bool> IsAlreadyParticipant(Guid boardId, string userId);
}