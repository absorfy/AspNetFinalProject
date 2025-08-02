using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface IBoardListService
{
    Task<IEnumerable<BoardList>> GetListsByBoardAsync(int boardId, string userId);
    Task<BoardList?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, string title);
    Task<BoardList> CreateAsync(int boardId, string title, string authorId);
    Task<bool> DeleteAsync(int id, string deletedByUserId);
}