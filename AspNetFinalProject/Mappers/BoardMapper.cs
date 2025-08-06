using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public class BoardMapper : EntityMapper<Board, CreateBoardDto, UpdateBoardDto, BoardDto>
{
    protected override BoardDto MapToDto(Board entity)
    {
        return new BoardDto
        {
            Id = entity.Id,
            WorkSpaceId = entity.WorkSpaceId,
            Title = entity.Title,
            Description = entity.Description,
            Visibility = entity.Visibility,
            AuthorName = entity.Author?.Username ?? entity.Author?.IdentityUser.UserName ?? "Unknown",
            ParticipantsCount = entity.Participants.Count,
            ListsIds = entity.Lists.Select(l => l.Id).ToList(),
        };
    }

    protected override void MapToEntity(Board entity, UpdateBoardDto updateDto)
    {
        entity.Title = updateDto.Title;
        entity.Description = updateDto.Description;
        entity.Visibility = updateDto.Visibility;
    }

    protected override Board MapToEntity(string authorId, CreateBoardDto createDto)
    {
        return new Board
        {
            WorkSpaceId = createDto.WorkSpaceId,
            Title = createDto.Title,
            AuthorId = authorId,
            Description = createDto.Description,
            Visibility = createDto.Visibility,
            CreatingTimestamp = DateTime.UtcNow
        };
    }
}