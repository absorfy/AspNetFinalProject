using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Mappers;

public static class CardMapper
{
    public static void UpdateEntity(Card card, UpdateCardDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(card);
        ArgumentNullException.ThrowIfNull(updateDto);

        card.Title = updateDto.Title;
        card.Description = updateDto.Description;
        card.Color = updateDto.Color;
        card.Deadline = updateDto.Deadline;
    }

    public static Card ToEntityFromCreateDto(CreateCardDto createDto)
    {
        ArgumentNullException.ThrowIfNull(createDto);
        return new Card
        {
            BoardListId = createDto.BoardListId,
            Title = createDto.Title,
            AuthorId = createDto.AuthorId,
            Description = createDto.Description,
            Color = createDto.Color,
            Deadline = createDto.Deadline,
            CreatingTimestamp = DateTime.UtcNow
        };
    }
}