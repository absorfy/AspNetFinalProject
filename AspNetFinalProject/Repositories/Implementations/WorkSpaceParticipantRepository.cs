using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;

namespace AspNetFinalProject.Repositories.Implementations;

public class WorkSpaceParticipantRepository : IWorkSpaceParticipantRepository
{
    private readonly ApplicationDbContext _context;

    public WorkSpaceParticipantRepository(ApplicationDbContext context)
    {
        _context = context;
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