using AspNetFinalProject.Common;
using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Repositories.Implementations;

public class UserActionLogRepository : IUserActionLogRepository
{
    private readonly ApplicationDbContext _context;

    public UserActionLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(UserActionLog log)
    {
        await _context.UserActionLogs.AddAsync(log);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserActionLog>> GetByUserIdAsync(string userId)
    {
        return await BaseQueryForUserId(userId).ToListAsync();
    }

    public async Task<PagedResult<UserActionLog>> GetByUserIdAsync(string userId, PagedRequest request)
    {
        var query = BaseQueryForUserId(userId, true);
        return await query.ToPagedResultAsync(request.Page, request.PageSize);
    }

    private IQueryable<UserActionLog> BaseQueryForUserId(string userId, bool asNoTracking = false)
    {
        var q = _context.UserActionLogs.Where(l => l.UserProfileId == userId);
        return asNoTracking ? q.AsNoTracking() : q;
    }
}