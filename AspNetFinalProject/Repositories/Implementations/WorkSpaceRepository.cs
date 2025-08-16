using AspNetFinalProject.Common;
using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Repositories.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Repositories.Implementations;

public class WorkSpaceRepository : IWorkSpaceRepository
{
    private readonly ApplicationDbContext _context;

    public WorkSpaceRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<WorkSpace>> GetUserWorkSpacesAsync(string userId)
    {
        return await BaseQueryForUser(userId).ToListAsync();
    }

    public async Task<PagedResult<WorkSpace>> GetUserWorkSpacesAsync(
        string userId, 
        PagedRequest request)
    {
        var query = BaseQueryForUser(userId, asNoTracking: true);

        // 🔍 Пошук
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var pattern = $"%{request.Search.Trim()}%";

            query = query.Where(w => EF.Functions.Like(w.Title, pattern) ||
                                     w.Description != null && EF.Functions.Like(w.Description, pattern) ||
                                     EF.Functions.Like(w.Author.Username, pattern));
        }
        
        query = (request.SortBy, request.Descending) switch
        {
            ("title", false) =>
                query.OrderBy(w => w.Title),
            ("title", true) =>
                query.OrderByDescending(w => w.Title),

            ("date", false) =>
                query.OrderBy(w => w.CreatingTimestamp),
            ("date", true) =>
                query.OrderByDescending(w => w.CreatingTimestamp),
            
            ("author", false) =>
                query.OrderBy(w => w.CreatingTimestamp),
            ("author", true) =>
                query.OrderByDescending(w => w.CreatingTimestamp),

            _ => query
        };

        return await query.ToPagedResultAsync(request.Page, request.PageSize);
    }
    
    private IQueryable<WorkSpace> BaseQueryForUser(string userId, bool asNoTracking = false)
    {
        var q = _context.WorkSpaces
            .Include(w => w.Author)
            .Include(w => w.Participants).ThenInclude(p => p.UserProfile)
            .Include(w => w.Boards)
            .Where(w => w.DeletedAt == null &&
                        (w.AuthorId == userId || w.Participants.Any(p => p.UserProfileId == userId)));

        return asNoTracking ? q.AsNoTracking() : q;
    }


    public async Task<WorkSpace?> GetByIdAsync(Guid workspaceId, bool withDeleted = false)
    {
        return await _context.WorkSpaces
            .Include(w => w.Author)
            .Include(w => w.Participants)
                .ThenInclude(p => p.UserProfile)
            .Include(w => w.Boards)
            .FirstOrDefaultAsync(w => w.Id == workspaceId && (withDeleted || w.DeletedAt == null));
    }

    public async Task AddAsync(WorkSpace workspace)
    {
        await _context.WorkSpaces.AddAsync(workspace);
    }
    
    public Task DeleteAsync(WorkSpace workspace)
    {
        // Soft delete
        workspace.DeletedAt = DateTime.UtcNow;
        _context.WorkSpaces.Update(workspace);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}