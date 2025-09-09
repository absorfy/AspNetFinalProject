using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Repositories.Implementations;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly ApplicationDbContext _context;

    public UserProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserProfile?> GetByIdentityId(string identityId)
    {
        return await _context.UserProfiles
            .Include(up => up.PersonalInfo)
            .Include(up => up.WorkspacesParticipating)
            .Include(up => up.BoardParticipants)
            .FirstOrDefaultAsync(up => up.IdentityId == identityId);
    }

    public async Task AddAsync(UserProfile userProfile)
    {
        await _context.UserProfiles.AddAsync(userProfile);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}