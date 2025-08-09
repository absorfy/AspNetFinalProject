using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;

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
}