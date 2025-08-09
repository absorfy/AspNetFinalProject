using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<Board>> GetBoardsByWorkSpaceAsync(Guid workSpaceId, string userId);
    Task<Board?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, UpdateBoardDto dto);
    Task<Board> CreateAsync(string authorId, CreateBoardDto dto);
    Task<bool> DeleteAsync(Guid id, string deletedByUserId);
}