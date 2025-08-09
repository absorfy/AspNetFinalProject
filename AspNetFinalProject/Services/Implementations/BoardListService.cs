using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Mappers;
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

    public async Task<IEnumerable<BoardList>> GetListsByBoardAsync(Guid boardId, string userId)
    {
        return await _repository.GetListsByBoardAsync(boardId, userId);
    }

    public async Task<BoardList?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(Guid id, UpdateBoardListDto dto)
    {
        var list = await _repository.GetByIdAsync(id);
        if (list == null) return false;
        BoardListMapper.UpdateEntity(list, dto);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<BoardList> CreateAsync(string authorId, CreateBoardListDto dto)
    {
        var list = BoardListMapper.CreateEntity(authorId, dto);
        await _repository.AddAsync(list);
        await _repository.SaveChangesAsync();
        return list;
    }

    public async Task<bool> DeleteAsync(Guid id, string deletedByUserId)
    {
        var list = await _repository.GetByIdAsync(id);
        if (list == null) return false;

        list.DeletedByUserId = deletedByUserId;
        await _repository.DeleteAsync(list);
        await _repository.SaveChangesAsync();

        return true;
    }
}