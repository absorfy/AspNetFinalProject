using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IBoardRepository
{
    Task<IEnumerable<Board>> GetBoardsByWorkSpaceAsync(int workSpaceId, string userId);
    Task<Board?> GetByIdAsync(int id);
    Task AddAsync(Board board);
    Task DeleteAsync(Board board);
    Task SaveChangesAsync();
}