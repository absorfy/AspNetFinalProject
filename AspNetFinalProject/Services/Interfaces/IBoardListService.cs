using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface IBoardListService
{
    Task<IEnumerable<BoardList>> GetListsByBoardAsync(Guid boardId, string userId);
    Task<BoardList?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, UpdateBoardListDto dto);
    Task<BoardList> CreateAsync(string authorId, CreateBoardListDto dto);
    Task<bool> DeleteAsync(Guid id, string deletedByUserId);
}