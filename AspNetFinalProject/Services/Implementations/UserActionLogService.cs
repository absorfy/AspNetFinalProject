using System.Text;
using AspNetFinalProject.Common;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class UserActionLogService : IUserActionLogService
{
    private readonly IUserActionLogRepository _actionLogRepository;
    private readonly IWorkSpaceRepository _workSpaceRepository;
    private readonly IBoardListRepository _boardListRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly ICardRepository _cardRepository;

    public UserActionLogService(
        IUserActionLogRepository actionLogRepository,
        IWorkSpaceRepository workSpaceRepository,
        IBoardListRepository boardListRepository,
        IBoardRepository boardRepository,
        ICardRepository cardRepository)
    {
        _actionLogRepository = actionLogRepository;
        _workSpaceRepository = workSpaceRepository;
        _boardListRepository = boardListRepository;
        _boardRepository = boardRepository;
        _cardRepository = cardRepository;
    }
    
    public async Task<UserActionLog> LogDeleting(string userId, ILogEntity logEntity)
    {
        return await LogAction(userId, logEntity, UserActionType.Delete,
            $"Видалено {logEntity.GetName()}" + (logEntity.GetParentLogEntity() != null ?
                $" з {logEntity.GetParentLogEntity()?.GetName()}" : ""));
    }

    public async Task<UserActionLog> LogUpdating(string userId, ILogEntity logEntity, params EntityUpdateLog[] updateLogs)
    {
        var message = new StringBuilder($"Змінено {logEntity.GetName()}" + (logEntity.GetParentLogEntity() != null ?
            $" в {logEntity.GetParentLogEntity()?.GetName()}" : ""));
        foreach (var updateLog in updateLogs)
        {
            message.Append($"\n{updateLog.ValueName}:\n\t{updateLog.OldValue} => {updateLog.NewValue}");
        }
        return await LogAction(userId, logEntity, UserActionType.Update, message.ToString());
    }

    public Task<UserActionLog> LogMoving(string userId, ILogEntity logEntity, EntityUpdateLog updateLog)
    {
        var message = $"Переміщено {logEntity.GetName()}" + (logEntity.GetParentLogEntity() != null ?
        $" в {logEntity.GetParentLogEntity()?.GetName()}" : "") +
                      $" з {updateLog.OldValue} до {updateLog.NewValue}.";
        return LogAction(userId, logEntity, UserActionType.Move, message);
    }


    public async Task<UserActionLog> LogCreating(string userId, ILogEntity logEntity)
    {
        return await LogAction(userId, logEntity, UserActionType.Create,
            $"Створено {logEntity.GetName()}" + (logEntity.GetParentLogEntity() != null ?
                $" в {logEntity.GetParentLogEntity()?.GetName()}" : ""));
    }

    public Task<IEnumerable<UserActionLog>> GetByUserIdAsync(string userId)
    {
        return _actionLogRepository.GetByUserIdAsync(userId);
    }

    public async Task<PagedResult<UserActionLog>> GetByUserIdAsync(string userId, PagedRequest request)
    {
        return await _actionLogRepository.GetByUserIdAsync(userId, request);
    }

    public async Task<string?> GetEntityLink(EntityTargetType entityType, Guid entityId)
    {
        ILogEntity? entityLog = entityType switch
        {
            EntityTargetType.Workspace => await _workSpaceRepository.GetByIdAsync(entityId, true),
            EntityTargetType.Board => await _boardRepository.GetByIdAsync(entityId, true),
            EntityTargetType.BoardList => await _boardListRepository.GetByIdAsync(entityId, true),
            EntityTargetType.Card => await _cardRepository.GetByIdAsync(entityId, true),
            _ => throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null)
        };
        return entityLog?.GetSettingsLink();
    }

    private async Task<UserActionLog> LogAction(
        string userId, 
        ILogEntity logEntity, 
        UserActionType actionType, 
        string message)
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
        return log;
    }
}