using AspNetFinalProject.Common;
using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Repositories.Implementations;

public class WorkSpaceParticipantRepository : IWorkSpaceParticipantRepository
{
    private readonly ApplicationDbContext _context;

    public WorkSpaceParticipantRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserProfile>> GetNonParticipantsAsync(Guid workSpaceId,
        string search)
    {
        return await _context.UserProfiles
            .Include(up => up.IdentityUser)
            .Include(up => up.PersonalInfo)
            .Where(up => up.WorkspacesParticipating.All(wp => wp.WorkSpaceId != workSpaceId))
            .Where(up => up.Username != null && up.Username.StartsWith(search) || 
                         up.IdentityUser.Email != null && up.IdentityUser.Email.StartsWith(search))
            .Take(20)
            .ToListAsync();

    }
    
    public async Task<IEnumerable<WorkSpaceParticipant>> GetByWorkSpaceIdAsync(Guid workSpaceId)
    {
        return await BaseQueryForWorkSpace(workSpaceId).ToListAsync();
    }

    public async Task<PagedResult<WorkSpaceParticipant>> GetByWorkSpaceIdAsync(Guid workSpaceId, PagedRequest request)
    {
        var query = BaseQueryForWorkSpace(workSpaceId, true);
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

    private IQueryable<WorkSpaceParticipant> BaseQueryForWorkSpace(Guid workSpaceId, bool asNoTracking = false)
    {
        var q = _context.WorkSpaceParticipants
            .Where(p => p.WorkSpaceId == workSpaceId)
            .Include(p => p.UserProfile);
        
        return asNoTracking ? q.AsNoTracking() : q;
    } 

    public async Task<WorkSpaceParticipant?> GetAsync(Guid workSpaceId, string userProfileId)
    {
        return await _context.WorkSpaceParticipants.FirstOrDefaultAsync(p => p.WorkSpaceId == workSpaceId && p.UserProfileId == userProfileId);
    }

    public async Task AddAsync(WorkSpaceParticipant participant)
    {
        await _context.WorkSpaceParticipants.AddAsync(participant);
    }

    public async Task RemoveAsync(Guid workspaceId, string participantId)
    {
        var participant = await _context.WorkSpaceParticipants
            .FirstOrDefaultAsync(p => p.WorkSpaceId == workspaceId && p.UserProfileId == participantId);

        if (participant != null)
        {
            _context.WorkSpaceParticipants.Remove(participant);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsAlreadyParticipant(Guid workspaceId, string userId)
    {
        return await _context.WorkSpaceParticipants.AnyAsync(wsp => wsp.WorkSpaceId == workspaceId && wsp.UserProfileId == userId);
    }
}