using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Hubs;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AspNetFinalProject.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IHubContext<NotificationsHub> _hub;

    public NotificationService(INotificationRepository notificationRepository,
                               IHubContext<NotificationsHub> hub)
    {
        _notificationRepository = notificationRepository;
        _hub = hub;
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

    public async Task CreateForSubscribersAsync(Guid userActionLogId, IEnumerable<string> subscriberIds)
    {
        var recipients = subscriberIds.Distinct().ToList();
        if (recipients.Count == 0) return;
        
        var notifications = recipients.Select(uid => new Notification
        {
            UserActionLogId = userActionLogId,
            UserProfileId = uid,
            IsRead = false,
            ReadAt = null
        }).ToList();

        await _notificationRepository.AddRangeAsync(notifications);
        await _notificationRepository.SaveChangesAsync();
        
        var dtos = notifications
            .Select(NotificationMapper.CreateDto)
            .ToList();

        foreach (var dto in dtos)
        {
            var group = $"user:{dto.ReceiverProfileId}";
            await _hub.Clients.Group(group).SendAsync("notificationCreated", dto);
        }
    }
}