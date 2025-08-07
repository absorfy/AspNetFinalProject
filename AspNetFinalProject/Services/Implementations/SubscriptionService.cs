using AspNetFinalProject.Entities;
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

    public async Task<bool> SubscribeAsync(string userId, string entityName, string entityId)
    {
        var existing = await _repository.GetAsync(userId, entityName, entityId);
        if (existing != null) return true;

        var subscription = new Subscription
        {
            UserProfileId = userId,
            EntityName = entityName,
            EntityId = entityId,
            Timestamp = DateTime.UtcNow
        };

        await _repository.AddAsync(subscription);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnsubscribeAsync(string userId, string entityName, string entityId)
    {
        var existing = await _repository.GetAsync(userId, entityName, entityId);
        if (existing == null) return false;

        await _repository.RemoveAsync(existing);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> IsSubscribedAsync(string userId, string entityName, string entityId)
    {
        var existing = await _repository.GetAsync(userId, entityName, entityId);
        return existing != null;
    }
}