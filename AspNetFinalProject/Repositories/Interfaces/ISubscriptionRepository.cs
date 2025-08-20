using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetAsync(string userId, EntityTargetType entityType, string entityId);
    Task<IEnumerable<string>> GetSubscribedIdsAsync(EntityTargetType? entityType, string entityId);
    Task AddAsync(Subscription subscription);
    Task RemoveAsync(Subscription subscription);
    Task SaveChangesAsync();
}