using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Repositories.Implementations;

public class BoardRepository : IBoardRepository
{
    private readonly ApplicationDbContext _context;

    public BoardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Board>> GetBoardsByWorkSpaceAsync(int workSpaceId, string userId)
    {
        return await _context.Boards
            .Include(b => b.Author)
            .Include(b => b.Participants)
            .ThenInclude(p => p.UserProfile)
            .Include(b => b.Lists)
            .Where(b => b.WorkSpaceId == workSpaceId 
                        && b.DeletedAt == null
                        && (b.AuthorId == userId || b.Participants.Any(p => p.UserProfileId == userId)))
            .ToListAsync();
    }

    public async Task<Board?> GetByIdAsync(int id)
    {
        return await _context.Boards
            .Include(b => b.Author)
            .Include(b => b.Participants)
            .ThenInclude(p => p.UserProfile)
            .Include(b => b.Lists)
            .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null);
    }

    public async Task AddAsync(Board board)
    {
        await _context.Boards.AddAsync(board);
    }

    public Task DeleteAsync(Board board)
    {
        // Soft delete
        board.DeletedAt = DateTime.UtcNow;
        _context.Boards.Update(board);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}