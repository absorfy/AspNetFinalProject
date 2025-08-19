using AspNetFinalProject.Common;
using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
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

    public async Task<IEnumerable<Board>> GetBoardsByWorkSpaceAsync(Guid workSpaceId, string userId)
    {
        return await BaseQueryForWorkSpace(workSpaceId, userId).ToListAsync();
    }

    public async Task<PagedResult<Board>> GetBoardsByWorkSpaceAsync(
        Guid workSpaceId, 
        string userId, 
        PagedRequest request)
    {
        var query = BaseQueryForWorkSpace(workSpaceId, userId, asNoTracking: true);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var pattern = $"%{request.Search.Trim()}%";
            query = query.Where(b => EF.Functions.Like(b.Title, pattern) ||
                                     b.Description != null && EF.Functions.Like(b.Description, pattern) ||
                                     EF.Functions.Like(b.Author.Username, pattern));
        }
        
        query = (request.SortBy, request.Descending) switch
        {
            ("title", false) =>
                query.OrderBy(b => b.Title),
            ("title", true) =>
                query.OrderByDescending(b => b.Title),

            ("date", false) =>
                query.OrderBy(b => b.CreatingTimestamp),
            ("date", true) =>
                query.OrderByDescending(b => b.CreatingTimestamp),
            
            ("author", false) =>
                query.OrderBy(b => b.CreatingTimestamp),
            ("author", true) =>
                query.OrderByDescending(b => b.CreatingTimestamp),

            _ => query
        };
        
        return await query.ToPagedResultAsync(request.Page, request.PageSize);
    }

    private IQueryable<Board> BaseQueryForWorkSpace(Guid workSpaceId, string userId, bool asNoTracking = false)
    {
        var workspace = _context.WorkSpaces.Include(workSpace => workSpace.Participants).FirstOrDefault(ws => ws.Id == workSpaceId);
        var isAdmin = workspace.Participants.Any(p => p.UserProfileId == userId && p.Role is ParticipantRole.Owner or ParticipantRole.Admin);
        var isWorkSpaceParticipant = workspace.Participants.Any(p => p.UserProfileId == userId);
        
        var q = _context.Boards
            .Include(b => b.Author)
            .Include(b => b.Participants)
            .ThenInclude(p => p.UserProfile)
            .Include(b => b.Lists)
            .Include(b => b.WorkSpace)
            .Where(b => b.WorkSpaceId == workSpaceId
                        && b.DeletedAt == null
                        && (b.AuthorId == userId || 
                            b.Participants.Any(p => p.UserProfileId == userId) ||
                            b.Visibility == BoardVisibility.Public ||
                            b.Visibility == BoardVisibility.Workspace && isWorkSpaceParticipant ||
                            b.Visibility == BoardVisibility.Private && isAdmin));

        return asNoTracking ? q.AsNoTracking() : q;
    }

    public async Task<Board?> GetByIdAsync(Guid id, bool withDeleted = false)
    {
        return await _context.Boards
            .Include(b => b.Author)
            .Include(b => b.Participants)
            .ThenInclude(p => p.UserProfile)
            .Include(b => b.Lists)
            .FirstOrDefaultAsync(b => b.Id == id && (withDeleted || b.DeletedAt == null));
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