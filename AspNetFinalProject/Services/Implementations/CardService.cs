using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class CardService : ICardService
{
    private readonly ICardRepository _repository;

    public CardService(ICardRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Card>> GetCardsByListAsync(int boardListId, string userId)
    {
        return await _repository.GetCardsByListAsync(boardListId, userId);
    }

    public async Task<Card?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(int id, string title, string? description, string? color, DateTime? deadline)
    {
        var card = await _repository.GetByIdAsync(id);
        if (card == null) return false;

        card.Title = title;
        card.Description = description;
        card.Color = color;
        card.Deadline = deadline;

        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<Card> CreateAsync(int boardListId, string title, string authorId, string? description = null, string? color = null, DateTime? deadline = null)
    {
        var card = new Card
        {
            BoardListId = boardListId,
            Title = title,
            AuthorId = authorId,
            Description = description,
            Color = color,
            Deadline = deadline,
            CreatingTimestamp = DateTime.UtcNow
        };

        await _repository.AddAsync(card);
        await _repository.SaveChangesAsync();

        return card;
    }

    public async Task<bool> DeleteAsync(int id, string deletedByUserId)
    {
        var card = await _repository.GetByIdAsync(id);
        if (card == null) return false;

        card.DeletedByUserId = deletedByUserId;
        await _repository.DeleteAsync(card);
        await _repository.SaveChangesAsync();

        return true;
    }
}