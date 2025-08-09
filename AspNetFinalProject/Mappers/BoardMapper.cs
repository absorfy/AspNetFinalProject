using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public static class BoardMapper
{
    public static BoardDto CreateDto(Board entity)
    {
        return new BoardDto
        {
            Id = entity.Id.ToString(),
            WorkSpaceId = entity.WorkSpaceId.ToString(),
            Title = entity.Title,
            Description = entity.Description,
            Visibility = entity.Visibility,
            AuthorName = entity.Author?.Username ?? entity.Author?.IdentityUser.UserName ?? "Unknown",
            ParticipantsCount = entity.Participants.Count,
            ListsIds = entity.Lists.Select(l => l.Id.ToString()).ToList(),
        };
    }

    public static void UpdateEntity(Board entity, UpdateBoardDto updateDto)
    {
        entity.Title = updateDto.Title;
        entity.Description = updateDto.Description;
        entity.Visibility = updateDto.Visibility;
    }

    public static Board CreateEntity(string authorId, CreateBoardDto createDto)
    {
        return new Board
        {
            WorkSpaceId = Guid.Parse(createDto.WorkSpaceId),
            Title = createDto.Title,
            AuthorId = authorId,
            Description = createDto.Description,
            Visibility = createDto.Visibility,
            CreatingTimestamp = DateTime.UtcNow
        };
    }
}