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

    public BoardService(IBoardRepository boardRepository,
                        IBoardParticipantRepository participantRepository,
                        IUserActionLogService userActionLogService)
    {
        _boardRepository = boardRepository;
        _participantRepository = participantRepository;
        _userActionLogService = userActionLogService;
    }

    public async Task<IEnumerable<Board>> GetAllByWorkSpaceAsync(Guid workSpaceId, string userId)
    {
        return await _boardRepository.GetBoardsByWorkSpaceAsync(workSpaceId, userId);
    }

    public async Task<Board?> GetByIdAsync(Guid id)
    {
        return await _boardRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(Guid id, UpdateBoardDto dto)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board == null) return false;
        BoardMapper.UpdateEntity(board, dto);
        await _boardRepository.SaveChangesAsync();
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

        await _userActionLogService.LogCreating(authorId, board);
        return await _boardRepository.GetByIdAsync(board.Id) ?? board;
    }

    public async Task<bool> DeleteAsync(Guid id, string deletedByUserId)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board == null) return false;

        board.DeletedByUserId = deletedByUserId;
        await _boardRepository.DeleteAsync(board);
        await _boardRepository.SaveChangesAsync();

        await _userActionLogService.LogDeleting(deletedByUserId, board);
        return true;
    }
}