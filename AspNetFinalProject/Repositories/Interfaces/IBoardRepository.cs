using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IBoardRepository
{
    Task<IEnumerable<Board>> GetBoardsByWorkSpaceAsync(Guid workSpaceId, string userId);
    Task<Board?> GetByIdAsync(Guid id);
    Task AddAsync(Board board);
    Task DeleteAsync(Board board);
    Task SaveChangesAsync();
}