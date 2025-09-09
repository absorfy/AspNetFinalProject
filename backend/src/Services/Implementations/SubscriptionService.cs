using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Repositories.Interfaces;
using AspNetFinalProject.Services.Interfaces;

namespace AspNetFinalProject.Services.Implementations;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _repository;

    public SubscriptionService(ISubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<string>> GetSubscribedAsync(EntityTargetType? entityTargetType, string entityId)
    {
        if (entityTargetType == null) return [];
        return await _repository.GetSubscribedIdsAsync(entityTargetType, entityId);
    }

    public async Task<bool> SubscribeAsync(string userId, EntityTargetType entityType, string entityId)
    {
        var existing = await _repository.GetAsync(userId, entityType, entityId);
        if (existing != null) return true;

        var subscription = new Subscription
        {
            UserProfileId = userId,
            EntityType = entityType,
            EntityId = entityId,
            Timestamp = DateTime.UtcNow
        };

        await _repository.AddAsync(subscription);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnsubscribeAsync(string userId, EntityTargetType entityType, string entityId)
    {
        var existing = await _repository.GetAsync(userId, entityType, entityId);
        if (existing == null) return false;

        await _repository.RemoveAsync(existing);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> IsSubscribedAsync(string userId, EntityTargetType entityType, string entityId)
    {
        var existing = await _repository.GetAsync(userId, entityType, entityId);
        return existing != null;
    }
}