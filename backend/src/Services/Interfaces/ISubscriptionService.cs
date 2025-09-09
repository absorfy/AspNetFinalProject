using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Services.Interfaces;

public interface ISubscriptionService
{
    Task<IEnumerable<string>> GetSubscribedAsync(EntityTargetType? entityTargetType, string entityId);
    Task<bool> SubscribeAsync(string userId, EntityTargetType entityType, string entityId);
    Task<bool> UnsubscribeAsync(string userId, EntityTargetType entityType, string entityId);
    Task<bool> IsSubscribedAsync(string userId, EntityTargetType entityType, string entityId);
}