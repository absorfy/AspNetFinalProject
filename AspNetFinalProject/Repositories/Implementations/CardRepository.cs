using AspNetFinalProject.Data;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Repositories.Implementations;

public class CardRepository : ICardRepository
{
    private readonly ApplicationDbContext _context;

    public CardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Card>> GetCardsByListAsync(Guid boardListId, string userId)
    {
        return await _context.Cards
            .Include(c => c.Author)
            .Include(c => c.Participants)
                .ThenInclude(p => p.UserProfile)
            .Include(c => c.TagCards)
                .ThenInclude(tc => tc.Tag)
            .Include(c => c.Comments)
            .Include(c => c.Attachments)
            .Where(c => c.BoardListId == boardListId
                        && c.DeletedAt == null
                        && (c.AuthorId == userId || c.Participants.Any(p => p.UserProfileId == userId)))
            .ToListAsync();
    }

    public async Task<Card?> GetByIdAsync(Guid id, bool withDeleted = false)
    {
        return await _context.Cards
            .Include(c => c.Author)
            .Include(c => c.Participants)
                .ThenInclude(p => p.UserProfile)
            .Include(c => c.TagCards)
                .ThenInclude(tc => tc.Tag)
            .Include(c => c.Comments)
            .Include(c => c.Attachments)
            .FirstOrDefaultAsync(c => c.Id == id && (withDeleted || c.DeletedAt == null));
    }

    public async Task AddAsync(Card card)
    {
        await _context.Cards.AddAsync(card);
    }

    public Task DeleteAsync(Card card)
    {
        // Soft delete
        card.DeletedAt = DateTime.UtcNow;
        _context.Cards.Update(card);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}