using AspNetFinalProject.Common;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Services.Interfaces;

public interface IUserActionLogService
{
    Task<UserActionLog> LogDeleting(string userId, ILogEntity logEntity);
    Task<UserActionLog> LogCreating(string userId, ILogEntity logEntity);

    Task<UserActionLog> LogUpdating(string userId, ILogEntity logEntity, params EntityUpdateLog[] updateLogs);
    Task<IEnumerable<UserActionLog>> GetByUserIdAsync(string userId);
    Task<string?> GetEntityLink(EntityTargetType entityType, Guid entityId);
}