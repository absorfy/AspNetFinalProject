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
        int page, 
        int pageSize,
        WorkSpaceSearchField searchField = WorkSpaceSearchField.None, 
        string? searchValue = null,
        WorkSpaceSortField sortField = WorkSpaceSortField.Title, 
        SortDirection sortDirection = SortDirection.Ascending,
        CancellationToken ct = default)
    {
        var query = BaseQueryForUser(userId, asNoTracking: true);

        // 🔍 Пошук
        if (!string.IsNullOrWhiteSpace(searchValue) && searchField != WorkSpaceSearchField.None)
        {
            var pattern = $"%{searchValue.Trim()}%";
            query = searchField switch
            {
                WorkSpaceSearchField.Title =>
                    query.Where(w => EF.Functions.Like(w.Title, pattern)),

                WorkSpaceSearchField.Description =>
                    query.Where(w => w.Description != null && EF.Functions.Like(w.Description, pattern)),

                WorkSpaceSearchField.AuthorName =>
                    query.Where(w => EF.Functions.Like(w.Author.Username, pattern)),

                _ => query
            };
        }

        // ↕ Сортування
        query = (sortField, sortDirection) switch
        {
            (WorkSpaceSortField.Title, SortDirection.Ascending) =>
                query.OrderBy(w => w.Title),
            (WorkSpaceSortField.Title, SortDirection.Descending) =>
                query.OrderByDescending(w => w.Title),

            (WorkSpaceSortField.CreatedAt, SortDirection.Ascending) =>
                query.OrderBy(w => w.CreatingTimestamp),
            (WorkSpaceSortField.CreatedAt, SortDirection.Descending) =>
                query.OrderByDescending(w => w.CreatingTimestamp),

            _ => query
        };

        return await query.ToPagedResultAsync(page, pageSize, ct);
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