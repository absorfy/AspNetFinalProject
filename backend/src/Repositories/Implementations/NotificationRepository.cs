using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Repositories.Implementations;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;
    
    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Notification>> GetAllByUserIdAsync(string userId, bool onlyUnread, int take)
    {
        var query = _context.Notifications
            .Include(n => n.UserActionLog)
            .Where(n => n.UserProfileId == userId);

        if (onlyUnread)
            query = query.Where(n => !n.IsRead);

        var list = await query
            .OrderByDescending(n => n.UserActionLog.Timestamp) // сортуємо вкінці
            .Take(take)
            .AsNoTracking()
            .ToListAsync();

        return list;
    }

    public async Task AddRangeAsync(IEnumerable<Notification> notifications)
    {
        await _context.Notifications.AddRangeAsync(notifications);
    }

    public async Task<Notification?> GetByIdAsync(Guid id)
    {
        return await _context.Notifications.FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}