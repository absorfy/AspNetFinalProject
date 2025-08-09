using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Mappers;
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

    public async Task<IEnumerable<Card>> GetCardsByListAsync(Guid boardListId, string userId)
    {
        return await _repository.GetCardsByListAsync(boardListId, userId);
    }

    public async Task<Card?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(Guid id, UpdateCardDto dto)
    {
        var card = await _repository.GetByIdAsync(id);
        if (card == null) return false;
        CardMapper.UpdateEntity(card, dto);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<Card> CreateAsync(string authorId, CreateCardDto dto)
    {
        var card = CardMapper.CreateEntity(authorId, dto);
        await _repository.AddAsync(card);
        await _repository.SaveChangesAsync();

        return card;
    }

    public async Task<bool> DeleteAsync(Guid id, string deletedByUserId)
    {
        var card = await _repository.GetByIdAsync(id);
        if (card == null) return false;

        card.DeletedByUserId = deletedByUserId;
        await _repository.DeleteAsync(card);
        await _repository.SaveChangesAsync();

        return true;
    }
}