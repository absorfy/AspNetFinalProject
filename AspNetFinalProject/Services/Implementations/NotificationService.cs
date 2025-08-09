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

    public Task<IEnumerable<Notification>> GetAllByUserId(string userId, bool onlyUnread, int take)
    {
        return _notificationRepository.GetAllByUserIdAsync(userId, onlyUnread, take);
    }

    public async Task<bool> MarkAsRead(Guid notificationId, string userId)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        if (notification == null || notification.IsRead || notification.UserProfileId != userId) return false;
        
        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        await _notificationRepository.SaveChangesAsync();
        return true;
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