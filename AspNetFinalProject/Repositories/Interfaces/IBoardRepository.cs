using AspNetFinalProject.Common;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IBoardRepository
{
    Task<IEnumerable<Board>> GetBoardsByWorkSpaceAsync(Guid workSpaceId, string userId);
    Task<PagedResult<Board>> GetBoardsByWorkSpaceAsync(Guid workSpaceId, string userId, PagedRequest request);
    Task<Board?> GetByIdAsync(Guid id, bool withDeleted = false);
    Task AddAsync(Board board);
    Task DeleteAsync(Board board);
    Task SaveChangesAsync();
}