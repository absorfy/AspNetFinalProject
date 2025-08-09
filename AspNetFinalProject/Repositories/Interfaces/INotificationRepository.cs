using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface INotificationRepository
{
    Task AddRangeAsync(IEnumerable<Notification> notifications);
    Task SaveChangesAsync();
}