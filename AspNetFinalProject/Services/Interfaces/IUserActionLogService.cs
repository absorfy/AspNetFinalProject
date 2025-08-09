using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Services.Interfaces;

public interface IUserActionLogService
{
    Task LogDeleting(string userId, ILogEntity logEntity);
    Task LogCreating(string userId, ILogEntity logEntity);
}