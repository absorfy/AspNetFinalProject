using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Repositories.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Repositories.Implementations;

public class BoardListRepository : IBoardListRepository
{
    private readonly ApplicationDbContext _context;

    public BoardListRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BoardList>> GetListsByBoardAsync(Guid boardId, string userId)
    {
        var board = _context.Boards.Include(board => board.Participants).FirstOrDefault(b => b.Id == boardId);
        var workspace = _context.WorkSpaces.Include(workSpace => workSpace.Participants).FirstOrDefault(ws => ws.Id == board.WorkSpaceId);
        var isAdmin = workspace.Participants.Any(p => p.UserProfileId == userId && p.Role is ParticipantRole.Owner or ParticipantRole.Admin);
        var isWorkSpaceParticipant = workspace.Participants.Any(p => p.UserProfileId == userId);
        
        return await _context.Lists
            .Include(l => l.Author)
            .Include(l => l.Cards)
            .Where(l => l.BoardId == boardId
                        && l.DeletedAt == null
                        && (l.AuthorId == userId || 
                            board.Participants.Any(p => p.UserProfileId == userId) ||
                            board.Visibility == BoardVisibility.Public ||
                            board.Visibility == BoardVisibility.Workspace && isWorkSpaceParticipant ||
                            board.Visibility == BoardVisibility.Private && isAdmin))
            .OrderBy(l => l.OrderIndex)
            .ToListAsync();
    }

    public async Task<BoardList?> GetByIdAsync(Guid id, bool withDeleted = false)
    {
        return await _context.Lists
            .Include(l => l.Author)
            .Include(l => l.Cards)
            .FirstOrDefaultAsync(l => l.Id == id && (withDeleted || l.DeletedAt == null));
    }

    public async Task AddAsync(BoardList list)
    {
        list.OrderIndex = _context.Lists.Count(l => l.BoardId == list.BoardId);
        await _context.Lists.AddAsync(list);
    }

    public async Task DeleteAsync(BoardList list)
    {
        // Soft delete
        list.DeletedAt = DateTime.UtcNow;
        list.OrderIndex = -1;
        _context.Lists.Update(list);
        var otherLists = await _context.Lists
            .Where(l => l.BoardId == list.BoardId && l.DeletedAt == null)
            .OrderBy(l => l.OrderIndex)
            .ToListAsync();
        
        for(var i = 0; i < otherLists.Count; i++)
        {
            otherLists[i].OrderIndex = i;
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}