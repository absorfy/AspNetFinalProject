using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Repositories.Implementations;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class UserActionLogService : IUserActionLogService
{
    private readonly UserActionLogRepository _actionLogRepository;

    public UserActionLogService(UserActionLogRepository actionLogRepository)
    {
        _actionLogRepository = actionLogRepository;
    }
    
    public async Task LogDeleting(string userId, ILogEntity logEntity)
    {
        await LogAction(userId, logEntity, UserActionType.Delete,
            $"Видалено дошку «{logEntity.GetName()}» у воркспейсі.");
    }

    

    public async Task LogCreating(string userId, ILogEntity logEntity)
    {
        await LogAction(userId, logEntity, UserActionType.Create,
            $"Створено дошку «{logEntity.GetName()}» у воркспейсі.");
    }
    
    private async Task LogAction(string userId, ILogEntity logEntity, UserActionType actionType, string message)
    {
        var log = new UserActionLog
        {
            UserProfileId = userId,
            EntityType = logEntity.GetEntityType(),
            EntityId = logEntity.GetId(),
            ActionType = actionType,
            Description = message,
            Timestamp = DateTime.UtcNow
        };
        
        await _actionLogRepository.AddAsync(log);
        await _actionLogRepository.SaveChangesAsync();
    }
}