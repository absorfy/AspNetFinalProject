using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<Notification>> GetAllByUserId(string userId, bool onlyUnread, int take);
    Task CreateForSubscribersAsync(Guid userActionLogId, IEnumerable<string> subscriberIds);
    Task<bool> MarkAsRead(Guid notificationId, string userId);
}