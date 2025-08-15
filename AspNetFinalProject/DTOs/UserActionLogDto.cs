using AspNetFinalProject.Enums;

namespace AspNetFinalProject.DTOs;

public class UserActionLogDto
{
    public string Id { get; set; }
    public int EntityType { get; set; }
    public string EntityId { get; set; }
    public int ActionType { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; }
}