using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class BoardListService : IBoardListService
{
    private readonly IBoardListRepository _repository;

    public BoardListService(IBoardListRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BoardList>> GetListsByBoardAsync(int boardId, string userId)
    {
        return await _repository.GetListsByBoardAsync(boardId, userId);
    }

    public async Task<BoardList?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(int id, string title)
    {
        var list = await _repository.GetByIdAsync(id);
        if (list == null) return false;

        list.Title = title;

        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<BoardList> CreateAsync(int boardId, string title, string authorId)
    {
        var list = new BoardList
        {
            BoardId = boardId,
            Title = title,
            AuthorId = authorId,
            CreatingTimestamp = DateTime.UtcNow
        };

        await _repository.AddAsync(list);
        await _repository.SaveChangesAsync();

        return list;
    }

    public async Task<bool> DeleteAsync(int id, string deletedByUserId)
    {
        var list = await _repository.GetByIdAsync(id);
        if (list == null) return false;

        list.DeletedByUserId = deletedByUserId;
        await _repository.DeleteAsync(list);
        await _repository.SaveChangesAsync();

        return true;
    }
}