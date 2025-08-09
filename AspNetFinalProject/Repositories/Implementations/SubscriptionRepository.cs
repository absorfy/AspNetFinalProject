using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Repositories.Implementations;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly ApplicationDbContext _context;
    
    public SubscriptionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Subscription?> GetAsync(string userId, EntityTargetType entityType, string entityId)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entityId))
        {
            throw new ArgumentException("User ID, entity name, and entity ID must be provided.");
        }

        return await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.UserProfileId == userId && s.EntityType == entityType && s.EntityId == entityId);
    }

    public async Task AddAsync(Subscription subscription)
    {
        if (subscription == null)
        {
            throw new ArgumentNullException(nameof(subscription), "Subscription cannot be null.");
        }

        if (string.IsNullOrEmpty(subscription.UserProfileId) || 
            string.IsNullOrEmpty(subscription.EntityId))
        {
            throw new ArgumentException("User ID, entity name, and entity ID must be provided.");
        }

        await _context.Subscriptions.AddAsync(subscription);
    }

    public Task RemoveAsync(Subscription subscription)
    {
        if (subscription == null)
        {
            throw new ArgumentNullException(nameof(subscription), "Subscription cannot be null.");
        }

        _context.Subscriptions.Remove(subscription);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}