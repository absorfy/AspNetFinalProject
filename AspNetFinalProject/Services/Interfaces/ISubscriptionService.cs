namespace AspNetFinalProject.Services.Interfaces;

public interface ISubscriptionService
{
    Task<bool> SubscribeAsync(string userId, string entityName, string entityId);
    Task<bool> UnsubscribeAsync(string userId, string entityName, string entityId);
    Task<bool> IsSubscribedAsync(string userId, string entityName, string entityId);
}