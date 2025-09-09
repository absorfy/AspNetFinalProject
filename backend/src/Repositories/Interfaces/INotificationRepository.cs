using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetAllByUserIdAsync(string userId, bool onlyUnread, int take);
    Task AddRangeAsync(IEnumerable<Notification> notifications);
    Task<Notification?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
}