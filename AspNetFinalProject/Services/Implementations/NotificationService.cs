using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }
    
    public async Task CreateForRecipientsAsync(Guid userActionLogId, IEnumerable<string> userProfileIds)
    {
        var now = DateTime.UtcNow;
        var notifications = userProfileIds.Distinct().Select(uid => new Notification
        {
            UserActionLogId = userActionLogId,
            UserProfileId = uid,
            IsRead = false,
            ReadAt = null
        });

        await _notificationRepository.AddRangeAsync(notifications);
        await _notificationRepository.SaveChangesAsync();
    }
}