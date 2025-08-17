using AspNetFinalProject.Common;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardParticipantRepository _participantRepository;
    private readonly ActionLogger _actionLogger;
    private readonly ISubscriptionService _subscriptionService;

    public BoardService(IBoardRepository boardRepository,
                        IBoardParticipantRepository participantRepository,
                        ActionLogger actionLogger,
                        ISubscriptionService subscriptionService)
    {
        _boardRepository = boardRepository;
        _participantRepository = participantRepository;
        _actionLogger = actionLogger;
        _subscriptionService = subscriptionService;
    }

    public async Task<IEnumerable<Board>> GetAllByWorkSpaceAsync(Guid workSpaceId, string userId)
    {
        return await _boardRepository.GetBoardsByWorkSpaceAsync(workSpaceId, userId);
    }

    public async Task<PagedResult<Board>> GetAllByWorkSpaceAsync(Guid workSpaceId, string userId, PagedRequest request)
    {
        return await _boardRepository.GetBoardsByWorkSpaceAsync(workSpaceId, userId, request);
    }

    public async Task<Board?> GetByIdAsync(Guid id)
    {
        return await _boardRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(Guid id, UpdateBoardDto dto, string updateByUserId)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board == null) return false;
        
        var updatingLogs = _actionLogger.CompareUpdateDtos(BoardMapper.CreateUpdateDto(board), dto).ToArray();
        if (updatingLogs.Length == 0) return false;
        
        BoardMapper.UpdateEntity(board, dto);
        await _boardRepository.SaveChangesAsync();

        await _actionLogger.LogAndNotifyAsync(updateByUserId, board, UserActionType.Update, updatingLogs);
        return true;
    }

    public async Task<Board> CreateAsync(string authorId, CreateBoardDto dto)
    {
        var board = BoardMapper.CreateEntity(authorId, dto);
        await _boardRepository.AddAsync(board);
        await _boardRepository.SaveChangesAsync();

        //Owner creating
        var owner = new BoardParticipant
        {
            UserProfileId = authorId,
            BoardId = board.Id,
            Role = ParticipantRole.Owner,
            JoiningTimestamp = DateTime.UtcNow
        };
        await _participantRepository.AddAsync(owner);
        await _participantRepository.SaveChangesAsync();

        await _actionLogger.LogAndNotifyAsync(authorId, board, UserActionType.Create);
        return await _boardRepository.GetByIdAsync(board.Id) ?? board;
    }

    public async Task<bool> DeleteAsync(Guid id, string deletedByUserId)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board == null) return false;

        board.DeletedByUserId = deletedByUserId;
        await _boardRepository.DeleteAsync(board);
        await _boardRepository.SaveChangesAsync();

        await _actionLogger.LogAndNotifyAsync(deletedByUserId, board, UserActionType.Delete);
        return true;
    }

    public async Task<bool> SubscribeAsync(Guid id, string userId)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board == null) return false;
        
        return await _subscriptionService.SubscribeAsync(userId, EntityTargetType.Board, id.ToString());
    }

    public async Task<bool> UnsubscribeAsync(Guid id, string userId)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board == null) return false;
        return await _subscriptionService.UnsubscribeAsync(userId, EntityTargetType.Board, id.ToString());
    }

    public Task<bool> IsSubscribedAsync(Guid id, string userId)
    {
        return _subscriptionService.IsSubscribedAsync(userId, EntityTargetType.Board, id.ToString());
    }
}