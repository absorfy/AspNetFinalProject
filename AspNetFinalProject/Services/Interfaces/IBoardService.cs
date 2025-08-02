using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Services.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<Board>> GetBoardsByWorkSpaceAsync(int workSpaceId, string userId);
    Task<Board?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, string title, string? description, BoardVisibility visibility);
    Task<Board> CreateAsync(int workSpaceId, string title, string authorId, string? description = null);
    Task<bool> DeleteAsync(int id, string deletedByUserId);
}