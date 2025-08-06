using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class CardService : ICardService
{
    private readonly ICardRepository _repository;
    private readonly CardMapper _mapper;

    public CardService(ICardRepository repository, CardMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Card>> GetCardsByListAsync(int boardListId, string userId)
    {
        return await _repository.GetCardsByListAsync(boardListId, userId);
    }

    public async Task<Card?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(int id, UpdateCardDto dto)
    {
        var card = await _repository.GetByIdAsync(id);
        if (card == null) return false;
        _mapper.UpdateEntity(card, dto);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<Card> CreateAsync(string authorId, CreateCardDto dto)
    {
        var card = _mapper.CreateEntity(authorId, dto);
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