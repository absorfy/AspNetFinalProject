using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public class BoardListMapper 
    : EntityMapper<BoardList, CreateBoardListDto, UpdateBoardListDto, BoardListDto>
{
    protected override BoardListDto MapToDto(BoardList entity)
    {
        return new BoardListDto
        {
            Id = entity.Id,
            BoardId = entity.BoardId,
            Title = entity.Title,
            AuthorName = entity.Author?.Username ?? entity.Author?.IdentityUser.UserName ?? "Unknown",
            CardsIds = entity.Cards.Select(card => card.Id).ToList(),
        };
    }

    protected override void MapToEntity(BoardList entity, UpdateBoardListDto updateDto)
    {
        entity.Title = updateDto.Title;
    }

    protected override BoardList MapToEntity(string authorId, CreateBoardListDto createDto)
    {
        return new BoardList
        {
            BoardId = createDto.BoardId,
            AuthorId = authorId,
            Title = createDto.Title,
            CreatingTimestamp = DateTime.UtcNow
        };
    }
}