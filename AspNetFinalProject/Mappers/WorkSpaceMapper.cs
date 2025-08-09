using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Mappers;

public static class WorkSpaceMapper
{
    public static WorkSpaceDto CreateDto(WorkSpace entity, bool isSubscribed = false)
    {
        return new WorkSpaceDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Visibility = entity.Visibility,
            AuthorName = entity.Author?.Username ?? entity.Author?.IdentityUser.UserName ?? "Unknown",
            IsSubscribed = isSubscribed,
            CreatingTimestamp = entity.CreatingTimestamp,
            BoardsCount = entity.Boards.Count,
            ParticipantIds = entity.Participants.Select(p => p.UserProfileId).ToList()
        };
    }

    public static void UpdateEntity(WorkSpace entity, UpdateWorkSpaceDto updateDto)
    {
        entity.Title = updateDto.Title;
        entity.Description = updateDto.Description;
        entity.Visibility = (WorkSpaceVisibility)updateDto.Visibility;
    }

    public static WorkSpace CreateEntity(string authorId, CreateWorkSpaceDto createDto)
    {
        return new WorkSpace
        {
            AuthorId = authorId,
            Title = createDto.Title,
            Description = createDto.Description,
            Visibility = (WorkSpaceVisibility)createDto.Visibility,
            CreatingTimestamp = DateTime.UtcNow
        };
    }
}