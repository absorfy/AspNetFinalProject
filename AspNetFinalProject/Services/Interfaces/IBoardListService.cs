using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface IBoardListService
{
    Task<IEnumerable<BoardList>> GetListsByBoardAsync(int boardId, string userId);
    Task<BoardList?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateBoardListDto dto);
    Task<BoardList> CreateAsync(string authorId, CreateBoardListDto dto);
    Task<bool> DeleteAsync(int id, string deletedByUserId);
}