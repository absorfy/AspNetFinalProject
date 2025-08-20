using AspNetFinalProject.Common;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class CardService : ICardService
{
    private readonly ICardRepository _repository;
    private readonly ActionLogger _actionLogger;

    public CardService(ICardRepository repository,
                       ActionLogger actionLogger)
    {
        _repository = repository;
        _actionLogger = actionLogger;
    }

    public async Task<IEnumerable<Card>> GetCardsByListAsync(Guid boardListId, string userId)
    {
        return await _repository.GetCardsByListAsync(boardListId, userId);
    }

    public async Task<Card?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(Guid id, UpdateCardDto dto, string updateByUserId)
    {
        var card = await _repository.GetByIdAsync(id);
        if (card == null) return false;

        var updateLogs = _actionLogger.CompareUpdateDtos(CardMapper.CreateUpdateDto(card), dto).ToArray();
        if (updateLogs.Length == 0) return false;
        CardMapper.UpdateEntity(card, dto);
        await _repository.SaveChangesAsync();

        var fullCard = await _repository.GetByIdAsync(card.Id);
        if(fullCard != null)
            await _actionLogger.LogAndNotifyAsync(updateByUserId, fullCard, UserActionType.Update, updateLogs);
        return true;
    }

    public async Task<Card> CreateAsync(string authorId, CreateCardDto dto)
    {
        var card = CardMapper.CreateEntity(authorId, dto);
        await _repository.AddAsync(card);
        await _repository.SaveChangesAsync();

        var fullCard = await _repository.GetByIdAsync(card.Id);
        if(fullCard != null)
            await _actionLogger.LogAndNotifyAsync(authorId, fullCard, UserActionType.Create);
        return card;
    }

    public async Task<bool> DeleteAsync(Guid id, string deletedByUserId, bool notify = true)
    {
        var card = await _repository.GetByIdAsync(id);
        if (card == null) return false;

        card.DeletedByUserId = deletedByUserId;
        await _repository.DeleteAsync(card);
        await _repository.SaveChangesAsync();
        
        if(notify)
            await _actionLogger.LogAndNotifyAsync(deletedByUserId, card, UserActionType.Delete);
        return true;
    }

    public async Task MoveCard(Guid cardId, Guid newListId, int orderIndex, string movedByUserId)
    {
        var card = await _repository.GetByIdAsync(cardId);
        if (card == null) return;
        var oldDto = CardMapper.CreateUpdateDto(card);
        
        await _repository.MoveCard(cardId, newListId, orderIndex);
        await _repository.SaveChangesAsync();
        
        var fullCard = await _repository.GetByIdAsync(cardId);

        if (fullCard != null)
        {
            var newDto = CardMapper.CreateUpdateDto(fullCard);
            var updateLogs = _actionLogger.CompareUpdateDtos(oldDto, newDto).ToArray();
            if (updateLogs.Length == 0) return;
            await _actionLogger.LogAndNotifyAsync(movedByUserId, fullCard, UserActionType.Move, updateLogs[0]);
        }
    }
}