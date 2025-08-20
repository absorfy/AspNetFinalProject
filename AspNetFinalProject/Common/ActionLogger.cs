using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Services.Interfaces;
using KellermanSoftware.CompareNetObjects;

namespace AspNetFinalProject.Common;

public class ActionLogger
{
    private readonly IUserActionLogService _userActionLogService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly INotificationService _notificationService;

    public ActionLogger(
        IUserActionLogService userActionLogService,
        ISubscriptionService subscriptionService,
        INotificationService notificationService)
    {
        _userActionLogService = userActionLogService;
        _subscriptionService = subscriptionService;
        _notificationService = notificationService;
    }
    
    public IEnumerable<EntityUpdateLog> CompareUpdateDtos(ILogUpdateDto oldDto, ILogUpdateDto newDto)
    {
        var updateLogs = new List<EntityUpdateLog>();
        var comparer = new CompareLogic
        {
            Config =
            {
                MaxDifferences = 10,
                ComparePrivateFields = false,
                ComparePrivateProperties = false
            }
        };

        var result = comparer.Compare(oldDto, newDto);

        foreach (var diff in result.Differences)
        {
            updateLogs.Add(new EntityUpdateLog
            {
                ValueName = diff.PropertyName,
                OldValue = diff.Object1Value ?? "Empty",
                NewValue = diff.Object2Value ?? "Empty"
            });
        }
        
        return updateLogs;
    }
    
    public async Task<UserActionLog?> LogAndNotifyAsync(
        string actorId,
        ILogEntity logEntity,
        UserActionType actionType,
        params EntityUpdateLog[] updateLogs)
    {
        UserActionLog log;
        switch (actionType)
        {
            case UserActionType.Create:
                log = await _userActionLogService.LogCreating(actorId, logEntity);
                await Notify(actorId, logEntity.GetParentLogEntity(), log);
                break;
            case UserActionType.Update:
                log = await _userActionLogService.LogUpdating(actorId, logEntity, updateLogs);
                await Notify(actorId, logEntity, log);
                break;
            case UserActionType.Delete:
                log = await _userActionLogService.LogDeleting(actorId, logEntity);
                if(!await Notify(actorId, logEntity.GetParentLogEntity(), log))
                    await Notify(actorId, logEntity, log);
                break;
            case UserActionType.Comment:
            case UserActionType.Move:
                log = await _userActionLogService.LogMoving(actorId, logEntity, updateLogs[0]);
                await Notify(actorId, logEntity.GetParentLogEntity(), log);
                break;
            case UserActionType.Assign:
            case UserActionType.None:
            default:
                throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
        }
        
        return log;
    }

    private async Task<bool> Notify(string actorId, ILogEntity? logEntity, UserActionLog log)
    {
        if (logEntity == null) return false;
        
        var wsSubs = await _subscriptionService.GetSubscribedAsync(logEntity.GetEntityType(), logEntity.GetId());
        wsSubs = wsSubs.Where(uid => uid != actorId).Distinct().ToList();

        if (wsSubs.Any())
        {
            await _notificationService.CreateForSubscribersAsync(log.Id, wsSubs);
            return true;
        }

        return false;
    }
}
