using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Repositories.Implementations;

public class BoardListRepository : IBoardListRepository
{
    private readonly ApplicationDbContext _context;

    public BoardListRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BoardList>> GetListsByBoardAsync(int boardId, string userId)
    {
        return await _context.Lists
            .Include(l => l.Author)
            .Include(l => l.Cards)
            .Where(l => l.BoardId == boardId
                        && l.DeletedAt == null
                        && (l.AuthorId == userId || l.Board.Participants.Any(p => p.UserProfileId == userId)))
            .ToListAsync();
    }

    public async Task<BoardList?> GetByIdAsync(int id)
    {
        return await _context.Lists
            .Include(l => l.Author)
            .Include(l => l.Cards)
            .FirstOrDefaultAsync(l => l.Id == id && l.DeletedAt == null);
    }

    public async Task AddAsync(BoardList list)
    {
        await _context.Lists.AddAsync(list);
    }

    public Task DeleteAsync(BoardList list)
    {
        // Soft delete
        list.DeletedAt = DateTime.UtcNow;
        _context.Lists.Update(list);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}