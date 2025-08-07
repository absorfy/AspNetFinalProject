using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetAsync(string userId, string entityName, string entityId);
    Task AddAsync(Subscription subscription);
    Task RemoveAsync(Subscription subscription);
    Task SaveChangesAsync();
}