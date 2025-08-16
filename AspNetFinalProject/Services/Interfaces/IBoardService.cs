using AspNetFinalProject.Common;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<Board>> GetAllByWorkSpaceAsync(Guid workSpaceId, string userId);
    Task<PagedResult<Board>> GetAllByWorkSpaceAsync(Guid workSpaceId, string userId, PagedRequest request);
    Task<Board?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, UpdateBoardDto dto, string updateByUserId);
    Task<Board> CreateAsync(string authorId, CreateBoardDto dto);
    Task<bool> DeleteAsync(Guid id, string deletedByUserId);
    Task<bool> SubscribeAsync(Guid id, string userId);
    Task<bool> UnsubscribeAsync(Guid id, string userId);
    Task<bool> IsSubscribedAsync(Guid id, string userId);
}