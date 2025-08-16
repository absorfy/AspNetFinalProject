using AspNetFinalProject.Common;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Services.Interfaces;

public interface IWorkSpaceService
{
    Task<PagedResult<WorkSpace>> GetUserWorkSpacesAsync(string userId, PagedRequest request);
    Task<IEnumerable<WorkSpace>> GetUserWorkSpacesAsync(string userId);
    Task<WorkSpace?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, UpdateWorkSpaceDto dto, string updatedByUserId);
    Task<WorkSpace> CreateAsync(string authorId, CreateWorkSpaceDto dto);
    Task<bool> DeleteAsync(Guid id, string deletedByUserId);
    Task<bool> SubscribeAsync(Guid id, string userId);
    Task<bool> UnsubscribeAsync(Guid id, string userId);
    Task<bool> IsSubscribedAsync(Guid id, string userId);
}