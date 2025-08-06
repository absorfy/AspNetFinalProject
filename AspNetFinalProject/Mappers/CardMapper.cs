using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public class CardMapper : EntityMapper<Card, CreateCardDto, UpdateCardDto, CardDto>
{

    protected override CardDto MapToDto(Card entity)
    {
        return new CardDto
        {
            Id = entity.Id,
            BoardListId = entity.BoardListId,
            Title = entity.Title,
            Description = entity.Description,
            Color = entity.Color,
            Deadline = entity.Deadline,
            AuthorName = entity.Author?.Username ?? entity.Author?.IdentityUser.UserName ?? "Unknown",
            ParticipantsCount = entity.Participants.Count,
            CommentsCount = entity.Comments.Count
        };
    }

    protected override void MapToEntity(Card entity, UpdateCardDto updateDto)
    {
        entity.Title = updateDto.Title;
        entity.Description = updateDto.Description;
        entity.Color = updateDto.Color;
        entity.Deadline = updateDto.Deadline;
    }

    protected override Card MapToEntity(string authorId, CreateCardDto createDto)
    {
        return new Card
        {
            BoardListId = createDto.BoardListId,
            Title = createDto.Title,
            AuthorId = authorId,
            Description = createDto.Description,
            Color = createDto.Color,
            Deadline = createDto.Deadline,
            CreatingTimestamp = DateTime.UtcNow
        };
    }
}