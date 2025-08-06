using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class BoardListService : IBoardListService
{
    private readonly IBoardListRepository _repository;
    private readonly BoardListMapper _mapper;

    public BoardListService(IBoardListRepository repository, BoardListMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BoardList>> GetListsByBoardAsync(int boardId, string userId)
    {
        return await _repository.GetListsByBoardAsync(boardId, userId);
    }

    public async Task<BoardList?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<bool> UpdateAsync(int id, UpdateBoardListDto dto)
    {
        var list = await _repository.GetByIdAsync(id);
        if (list == null) return false;
        _mapper.UpdateEntity(list, dto);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<BoardList> CreateAsync(string authorId, CreateBoardListDto dto)
    {
        var list = _mapper.CreateEntity(authorId, dto);
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