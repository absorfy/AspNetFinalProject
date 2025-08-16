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
                break;
            case UserActionType.Update:
                log = await _userActionLogService.LogUpdating(actorId, logEntity, updateLogs);
                break;
            case UserActionType.Delete:
                log = await _userActionLogService.LogDeleting(actorId, logEntity);
                break;
            case UserActionType.Comment:
            case UserActionType.Move:
            case UserActionType.Assign:
            case UserActionType.None:
            default:
                throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
        }

        var wsSubs = await _subscriptionService.GetSubscribedAsync(logEntity.GetEntityType(), logEntity.GetId());
        wsSubs = wsSubs.Where(uid => uid != actorId).Distinct().ToList();

        if (wsSubs.Any())
            await _notificationService.CreateForSubscribersAsync(log.Id, wsSubs);
        return log;
    }
}
