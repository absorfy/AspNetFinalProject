using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
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
        return await _context.WorkSpaces
            .Include(w => w.Author)
            .Include(w => w.Participants)
                .ThenInclude(p => p.UserProfile)
            .Include(w => w.Boards)
            .Where(w => w.DeletedAt == null && 
                        (w.AuthorId == userId || w.Participants.Any(p => p.UserProfileId == userId)))
            .ToListAsync();
    }


    public async Task<WorkSpace?> GetByIdAsync(int workspaceId)
    {
        return await _context.WorkSpaces
            .Include(w => w.Author)
            .Include(w => w.Participants)
                .ThenInclude(p => p.UserProfile)
            .Include(w => w.Boards)
            .FirstOrDefaultAsync(w => w.Id == workspaceId && w.DeletedAt == null);
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