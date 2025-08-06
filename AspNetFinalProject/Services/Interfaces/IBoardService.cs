using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<Board>> GetBoardsByWorkSpaceAsync(int workSpaceId, string userId);
    Task<Board?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateBoardDto dto);
    Task<Board> CreateAsync(string authorId, CreateBoardDto dto);
    Task<bool> DeleteAsync(int id, string deletedByUserId);
}