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
    private readonly IUserActionLogService _userActionLogService;
    private readonly ActionLogger _actionLogger;

    public BoardService(IBoardRepository boardRepository,
                        IBoardParticipantRepository participantRepository,
                        IUserActionLogService userActionLogService,
                        ActionLogger actionLogger)
    {
        _boardRepository = boardRepository;
        _participantRepository = participantRepository;
        _userActionLogService = userActionLogService;
        _actionLogger = actionLogger;
    }

    public async Task<IEnumerable<Board>> GetAllByWorkSpaceAsync(Guid workSpaceId, string userId)
    {
        return await _boardRepository.GetBoardsByWorkSpaceAsync(workSpaceId, userId);
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

        //Admin creating
        var admin = new BoardParticipant
        {
            UserProfileId = authorId,
            BoardId = board.Id,
            Role = BoardRole.Admin,
            JoiningTimestamp = DateTime.UtcNow
        };
        await _participantRepository.AddAsync(admin);
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
}