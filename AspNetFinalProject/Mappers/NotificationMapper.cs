using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public static class NotificationMapper
{
    public static NotificationDto CreateDto(Notification notification)
    {
        return new NotificationDto
        {
            Id = notification.Id.ToString(),
            IsRead = notification.IsRead,
            ReadAt = notification.ReadAt,
            UserActionLog = UserActionLogMapper.CreateDto(notification.UserActionLog),
        };
    }
}