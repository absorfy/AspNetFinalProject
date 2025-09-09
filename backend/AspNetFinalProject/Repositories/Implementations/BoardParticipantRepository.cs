using AspNetFinalProject.Common;
using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Repositories.Implementations;

public class BoardParticipantRepository : IBoardParticipantRepository
{
    private readonly ApplicationDbContext _context;
    
    public BoardParticipantRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(BoardParticipant participant)
    {
        await _context.BoardParticipants.AddAsync(participant);
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserProfile>> GetNonParticipantsAsync(Guid boardId,
        string search)
    {
        return await _context.UserProfiles
            .Include(up => up.IdentityUser)
            .Include(up => up.PersonalInfo)
            .Where(up => up.BoardParticipants.All(bp => bp.BoardId != boardId))
            .Where(up => up.Username != null && up.Username.StartsWith(search) || 
                         up.IdentityUser.Email != null && up.IdentityUser.Email.StartsWith(search))
            .Take(20)
            .ToListAsync();

    }

    public async Task<BoardParticipant?> GetAsync(Guid boardId, string userProfileId)
    {
        return await _context.BoardParticipants.Include(bp => bp.UserProfile)
            .FirstOrDefaultAsync(p => p.BoardId == boardId && p.UserProfileId == userProfileId);
    }

    public async Task<PagedResult<BoardParticipant>> GetByBoardIdAsync(Guid boardId, PagedRequest request)
    {
        var query = BaseQueryForBoard(boardId, true);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var pattern = $"%{request.Search.Trim()}%";
            query = query.Where(wp => EF.Functions.Like(wp.UserProfile.Username, pattern));
        }
        
        query = (request.SortBy, request.Descending) switch
        {
            ("username", false) =>
                query.OrderBy(wp => wp.UserProfile.Username),
            ("username", true) =>
                query.OrderByDescending(wp => wp.UserProfile.Username),

            ("date", false) =>
                query.OrderBy(wp => wp.JoiningTimestamp),
            ("date", true) =>
                query.OrderByDescending(wp => wp.JoiningTimestamp),

            _ => query
        };
        
        return await query.ToPagedResultAsync(request.Page, request.PageSize);
    }

    public async Task RemoveAsync(Guid boardId, string participantId)
    {
        var participant = await _context.BoardParticipants
            .FirstOrDefaultAsync(p => p.BoardId == boardId && p.UserProfileId == participantId);

        if (participant != null)
        {
            _context.BoardParticipants.Remove(participant);
        }
    }

    public async Task<bool> IsAlreadyParticipant(Guid boardId, string userId)
    {
        return await _context.BoardParticipants.AnyAsync(bp => bp.BoardId == boardId && bp.UserProfileId == userId);
    }

    private IQueryable<BoardParticipant> BaseQueryForBoard(Guid boardId, bool asNoTracking = false)
    {
        var q = _context.BoardParticipants
            .Where(p => p.BoardId == boardId)
            .Include(p => p.UserProfile);
        
        return asNoTracking ? q.AsNoTracking() : q;
    } 
}