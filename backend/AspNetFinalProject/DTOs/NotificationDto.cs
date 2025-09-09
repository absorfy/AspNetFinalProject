using AspNetFinalProject.Enums;

namespace AspNetFinalProject.DTOs;

public class NotificationDto
{
    public string Id { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public UserActionLogDto UserActionLog { get; set; } 
    public string ReceiverProfileId { get; set; }
}