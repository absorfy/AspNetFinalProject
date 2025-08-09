using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<Notification>> GetAllByUserId(string userId, bool onlyUnread, int take);
    Task CreateForRecipientsAsync(Guid userActionLogId, IEnumerable<string> userProfileIds);
    Task<bool> MarkAsRead(Guid notificationId, string userId);
}