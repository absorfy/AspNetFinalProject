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
            EntityType = (int)userActionLog.EntityType,
            EntityId = userActionLog.EntityId,
            ActionType = (int)userActionLog.ActionType,
            Description = userActionLog.Description,
            Timestamp = userActionLog.Timestamp,
        };
    }
}