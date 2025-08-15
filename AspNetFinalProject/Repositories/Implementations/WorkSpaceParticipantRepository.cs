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

    public async Task<IEnumerable<UserProfile>> GetNonParticipantsByUserNameAsync(Guid workSpaceId,
        string userName)
    {
        return await _context.UserProfiles
            .Include(up => up.IdentityUser)
            .Include(up => up.PersonalInfo)
            .Where(up => up.WorkspacesParticipating.All(wp => wp.WorkSpaceId != workSpaceId))
            .Where(up => up.Username != null && up.Username.StartsWith(userName))
            .Take(20)
            .ToListAsync();

    }
    
    public async Task<IEnumerable<UserProfile>> GetNonParticipantsByEmailAsync(Guid workSpaceId,
        string email)
    {
        return await _context.UserProfiles
            .Include(up => up.IdentityUser)
            .Include(up => up.PersonalInfo)
            .Where(up => up.WorkspacesParticipating.All(wp => wp.WorkSpaceId != workSpaceId))
            .Where(up => up.IdentityUser.Email != null && up.IdentityUser.Email.StartsWith(email))
            .Take(20)
            .ToListAsync();

    }
    
    public async Task<IEnumerable<WorkSpaceParticipant>> GetByWorkSpaceIdAsync(Guid workSpaceId)
    {
        return await _context.WorkSpaceParticipants
            .Where(p => p.WorkSpaceId == workSpaceId)
            .Include(p => p.UserProfile)
            .ToListAsync();
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