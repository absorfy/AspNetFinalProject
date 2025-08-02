using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _repository;

    public BoardService(IBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Board>> GetBoardsByWorkSpaceAsync(int workSpaceId, string userId)
    {
        return await _repository.GetBoardsByWorkSpaceAsync(workSpaceId, userId);
    }

    public async Task<Board?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(int id, string title, string? description, BoardVisibility visibility)
    {
        var board = await _repository.GetByIdAsync(id);
        if (board == null) return false;

        board.Title = title;
        board.Description = description;
        board.Visibility = visibility;

        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<Board> CreateAsync(int workSpaceId, string title, string authorId, string? description = null)
    {
        var board = new Board
        {
            WorkSpaceId = workSpaceId,
            Title = title,
            AuthorId = authorId,
            Description = description,
            CreatingTimestamp = DateTime.UtcNow
        };

        await _repository.AddAsync(board);
        await _repository.SaveChangesAsync();

        return board;
    }

    public async Task<bool> DeleteAsync(int id, string deletedByUserId)
    {
        var board = await _repository.GetByIdAsync(id);
        if (board == null) return false;

        board.DeletedByUserId = deletedByUserId;
        await _repository.DeleteAsync(board);
        await _repository.SaveChangesAsync();

        return true;
    }
}