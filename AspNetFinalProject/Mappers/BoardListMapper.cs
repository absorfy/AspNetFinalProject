using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public static class BoardListMapper
{
    public static BoardListDto CreateDto(BoardList entity)
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

    public static void UpdateEntity(BoardList entity, UpdateBoardListDto updateDto)
    {
        entity.Title = updateDto.Title;
    }

    public static BoardList CreateEntity(string authorId, CreateBoardListDto createDto)
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