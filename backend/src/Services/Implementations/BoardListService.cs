using AspNetFinalProject.Common;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class BoardListService : IBoardListService
{
    private readonly IBoardListRepository _repository;
    private readonly ActionLogger _actionLogger;
    private readonly ICardService _cardService;

    public BoardListService(IBoardListRepository repository,
                            ActionLogger actionLogger, 
                            ICardService cardService)
    {
        _repository = repository;
        _actionLogger = actionLogger;
        _cardService = cardService;
    }

    public async Task<IEnumerable<BoardList>> GetListsByBoardAsync(Guid boardId, string userId)
    {
        return await _repository.GetListsByBoardAsync(boardId, userId);
    }

    public async Task<BoardList?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(Guid id, UpdateBoardListDto dto, string updatedByUserId)
    {
        var list = await _repository.GetByIdAsync(id);
        if (list == null) return false;

        var updateLogs = _actionLogger.CompareUpdateDtos(BoardListMapper.CreateUpdateDto(list), dto).ToArray();
        if (updateLogs.Length == 0) return false;
        
        BoardListMapper.UpdateEntity(list, dto);
        await _repository.SaveChangesAsync();
        
        await _actionLogger.LogAndNotifyAsync(updatedByUserId, list, UserActionType.Update, updateLogs);
        return true;
    }

    public async Task<BoardList> CreateAsync(string authorId, CreateBoardListDto dto)
    {
        var list = BoardListMapper.CreateEntity(authorId, dto);
        await _repository.AddAsync(list);
        await _repository.SaveChangesAsync();

        var fullList = await _repository.GetByIdAsync(list.Id);
        if (fullList == null) return list;
        await _actionLogger.LogAndNotifyAsync(authorId, fullList, UserActionType.Create);
        return list;
    }

    public async Task<bool> DeleteAsync(Guid id, string deletedByUserId, bool notify = true)
    {
        var list = await _repository.GetByIdAsync(id);
        if (list == null) return false;

        list.DeletedByUserId = deletedByUserId;
        await _repository.DeleteAsync(list);
        await _repository.SaveChangesAsync();

        foreach (var card in list.Cards)
        {
            await _cardService.DeleteAsync(card.Id, deletedByUserId, false);
        }
        
        if(notify) 
            await _actionLogger.LogAndNotifyAsync(deletedByUserId, list, UserActionType.Delete);
        return true;
    }
}