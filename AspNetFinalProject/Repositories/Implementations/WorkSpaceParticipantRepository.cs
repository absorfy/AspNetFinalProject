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

    public async Task<IEnumerable<WorkSpaceParticipant>> GetByWorkSpaceIdAsync(Guid workSpaceId)
    {
        return await _context.WorkSpaceParticipants
            .Where(p => p.WorkSpaceId == workSpaceId)
            .Include(p => p.UserProfile)
            .ToListAsync();
    }

    public async Task AddAsync(WorkSpaceParticipant participant)
    {
        await _context.WorkSpaceParticipants.AddAsync(participant);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}