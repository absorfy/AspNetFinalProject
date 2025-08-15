using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Services.Interfaces;

public interface IUserActionLogService
{
    Task<UserActionLog> LogDeleting(string userId, ILogEntity logEntity);
    Task<UserActionLog> LogCreating(string userId, ILogEntity logEntity);
    Task<IEnumerable<UserActionLog>> GetByUserIdAsync(string userId);
}