using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public class WorkSpaceMapper 
    : EntityMapper<WorkSpace, CreateWorkSpaceDto, UpdateWorkSpaceDto, WorkSpaceDto>
{
    protected override WorkSpaceDto MapToDto(WorkSpace entity)
    {
        return new WorkSpaceDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Visibility = entity.Visibility,
            AuthorName = entity.Author?.Username ?? entity.Author?.IdentityUser.UserName ?? "Unknown",
            ParticipantsCount = entity.Participants.Count,
            BoardsCount = entity.Boards.Count
        };
    }

    protected override void MapToEntity(WorkSpace entity, UpdateWorkSpaceDto updateDto)
    {
        entity.Title = updateDto.Title;
        entity.Description = updateDto.Description;
        entity.Visibility = updateDto.Visibility;
    }

    protected override WorkSpace MapToEntity(string authorId, CreateWorkSpaceDto createDto)
    {
        return new WorkSpace
        {
            AuthorId = authorId,
            Title = createDto.Title,
            Description = createDto.Description,
            Visibility = createDto.Visibility,
            CreatingTimestamp = DateTime.UtcNow
        };
    }
}