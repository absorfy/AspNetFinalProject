using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Repositories.Implementations;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;
    
    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task AddRangeAsync(IEnumerable<Notification> notifications)
    {
        await _context.Notifications.AddRangeAsync(notifications);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}