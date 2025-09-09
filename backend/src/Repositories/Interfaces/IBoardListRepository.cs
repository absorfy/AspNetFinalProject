using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface  IBoardListRepository
{
    Task<IEnumerable<BoardList>> GetListsByBoardAsync(Guid boardId, string userId);
    Task<BoardList?> GetByIdAsync(Guid id, bool withDeleted = false);
    Task AddAsync(BoardList list);
    Task DeleteAsync(BoardList list);
    Task SaveChangesAsync();
}