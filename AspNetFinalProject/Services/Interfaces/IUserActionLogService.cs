using AspNetFinalProject.Common;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Services.Interfaces;

public interface IUserActionLogService
{
    Task<UserActionLog> LogDeleting(string userId, ILogEntity logEntity);
    Task<UserActionLog> LogCreating(string userId, ILogEntity logEntity);

    Task<UserActionLog> LogUpdating(string userId, ILogEntity logEntity, params EntityUpdateLog[] updateLogs);
    Task<UserActionLog> LogMoving(string userId, ILogEntity logEntity, EntityUpdateLog updateLog);
    Task<IEnumerable<UserActionLog>> GetByUserIdAsync(string userId);
    Task<PagedResult<UserActionLog>> GetByUserIdAsync(string userId, PagedRequest request);
    Task<string?> GetEntityLink(EntityTargetType entityType, Guid entityId);
}