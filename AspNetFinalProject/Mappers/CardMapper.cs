using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Mappers;

public static class CardMapper
{

    public static CardDto CreateDto(Card entity, ParticipantRole? userBoardRole)
    {
        return new CardDto
        {
            Id = entity.Id.ToString(),
            BoardListId = entity.BoardListId.ToString(),
            Title = entity.Title,
            Description = entity.Description,
            Color = entity.Color,
            Deadline = entity.Deadline,
            AuthorName = entity.Author?.Username ?? entity.Author?.IdentityUser.UserName ?? "Unknown",
            ParticipantsCount = entity.Participants.Count,
            CommentsCount = entity.Comments.Count,
            UserBoardRole = userBoardRole,
        };
    }

    public static UpdateCardDto CreateUpdateDto(Card entity)
    {
        return new UpdateCardDto
        {
            Color = entity.Color,
            Description = entity.Description,
            Deadline = entity.Deadline,
            Title = entity.Title,
        };
    }

    public static void UpdateEntity(Card entity, UpdateCardDto updateDto)
    {
        entity.Title = updateDto.Title;
        entity.Description = updateDto.Description;
        entity.Color = updateDto.Color;
        entity.Deadline = updateDto.Deadline;
    }

    public static Card CreateEntity(string authorId, CreateCardDto createDto)
    {
        return new Card
        {
            BoardListId = Guid.Parse(createDto.BoardListId),
            Title = createDto.Title,
            AuthorId = authorId,
            Description = createDto.Description,
            Color = createDto.Color,
            Deadline = createDto.Deadline,
            CreatingTimestamp = DateTime.UtcNow
        };
    }
}