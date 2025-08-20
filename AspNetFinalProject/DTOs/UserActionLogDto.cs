using AspNetFinalProject.Enums;

namespace AspNetFinalProject.DTOs;

public class UserActionLogDto
{
    public string Id { get; set; }
    public EntityTargetType EntityType { get; set; }
    public string UserName { get; set; }
    public string EntityId { get; set; }
    public UserActionType ActionType { get; set; }
    public IEnumerable<string> Messages { get; set; }
    public DateTime Timestamp { get; set; }
}