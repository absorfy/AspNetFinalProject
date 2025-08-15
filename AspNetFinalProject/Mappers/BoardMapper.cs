using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

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
            Visibility = (int)entity.Visibility,
            AuthorName = entity.Author?.Username ?? entity.Author?.IdentityUser.UserName ?? "Unknown",
            ParticipantsCount = entity.Participants.Count,
            ListsIds = entity.Lists.Select(l => l.Id.ToString()).ToList(),
        };
    }

    public static void UpdateEntity(Board entity, UpdateBoardDto updateDto)
    {
        entity.Title = updateDto.Title;
        entity.Description = updateDto.Description;
        if (Enum.TryParse(updateDto.Visibility.ToString(), out BoardVisibility visibility))
        {
            entity.Visibility = visibility;
        }
        else throw new Exception("Invalid visibility value");
    }

    public static Board CreateEntity(string authorId, CreateBoardDto createDto)
    {
        if (Enum.TryParse(createDto.Visibility.ToString(), out BoardVisibility visibility))
        {
            return new Board
            {
                WorkSpaceId = Guid.Parse(createDto.WorkSpaceId),
                Title = createDto.Title,
                AuthorId = authorId,
                Description = createDto.Description,
                Visibility = visibility,
                CreatingTimestamp = DateTime.UtcNow
            };
        }
        throw new Exception("Invalid visibility value");
    }
}