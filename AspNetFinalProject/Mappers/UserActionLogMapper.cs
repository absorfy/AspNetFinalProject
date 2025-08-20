using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public static class UserActionLogMapper
{
    public static UserActionLogDto CreateDto(UserActionLog userActionLog)
    {
        return new UserActionLogDto
        {
            Id = userActionLog.Id.ToString(),
            EntityType = userActionLog.EntityType,
            EntityId = userActionLog.EntityId,
            ActionType = userActionLog.ActionType,
            Messages = userActionLog.Description
                .Replace("\t", " -  ")
                .Split('\n', StringSplitOptions.RemoveEmptyEntries),
            Timestamp = userActionLog.Timestamp,
            UserName = userActionLog.UserProfile?.Username ?? "Unknown User",
        };
    }
}