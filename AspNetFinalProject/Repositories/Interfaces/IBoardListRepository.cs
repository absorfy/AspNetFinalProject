using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface  IBoardListRepository
{
    Task<IEnumerable<BoardList>> GetListsByBoardAsync(int boardId, string userId);
    Task<BoardList?> GetByIdAsync(int id);
    Task AddAsync(BoardList list);
    Task DeleteAsync(BoardList list);
    Task SaveChangesAsync();
}