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

    public BoardService(IBoardRepository boardRepository,
                        IBoardParticipantRepository participantRepository)
    {
        _boardRepository = boardRepository;
        _participantRepository = participantRepository;
    }

    public async Task<IEnumerable<Board>> GetBoardsByWorkSpaceAsync(int workSpaceId, string userId)
    {
        return await _boardRepository.GetBoardsByWorkSpaceAsync(workSpaceId, userId);
    }

    public async Task<Board?> GetByIdAsync(int id)
    {
        return await _boardRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(int id, UpdateBoardDto dto)
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

        var admin = new BoardParticipant
        {
            UserProfileId = authorId,
            BoardId = board.Id,
            Role = BoardRole.Admin,
            JoiningTimestamp = DateTime.UtcNow
        };

        await _participantRepository.AddAsync(admin);
        await _participantRepository.SaveChangesAsync();
        
        return await _boardRepository.GetByIdAsync(board.Id) ?? board;
    }

    public async Task<bool> DeleteAsync(int id, string deletedByUserId)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board == null) return false;

        board.DeletedByUserId = deletedByUserId;
        await _boardRepository.DeleteAsync(board);
        await _boardRepository.SaveChangesAsync();

        return true;
    }
}