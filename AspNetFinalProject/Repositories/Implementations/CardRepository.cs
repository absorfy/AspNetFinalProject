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
            .OrderBy(c => c.OrderIndex)
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
        card.OrderIndex = _context.Cards.Count(c => c.BoardListId == card.BoardListId);
        await _context.Cards.AddAsync(card);
    }

    public async Task DeleteAsync(Card card)
    {
        // Soft delete
        card.DeletedAt = DateTime.UtcNow;
        card.OrderIndex = -1;
        _context.Cards.Update(card);
        var otherCards = await _context.Cards
            .Where(c => c.BoardListId == card.BoardListId && c.DeletedAt == null)
            .OrderBy(c => c.OrderIndex)
            .ToListAsync();
        
        for (var i = 0; i < otherCards.Count; i++)
        {
            otherCards[i].OrderIndex = i;
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task MoveCard(Guid cardId, Guid newListId, int orderIndex)
    {
        var card = await GetByIdAsync(cardId);
        if (card == null) return;
        
        var newListCards = await _context.Cards
            .Where(c => c.BoardListId == newListId && c.DeletedAt == null)
            .OrderBy(c => orderIndex)
            .ToListAsync();
        
        if(orderIndex < 0 || 
           orderIndex > newListCards.Count) return;
        
        card.OrderIndex = orderIndex;
        
        for (var i = orderIndex; i < newListCards.Count; i++)
        {
            newListCards[i].OrderIndex = i + 1;
        }
        
        var oldListCards = await _context.Cards
            .Where(c => c.BoardListId == card.BoardListId 
                        && c.Id != card.Id && c.DeletedAt == null)
            .OrderBy(c => orderIndex)
            .ToListAsync();

        for (var i = 0; i < oldListCards.Count; i++)
        {
            oldListCards[i].OrderIndex = i;
        }
            
        card.BoardListId = newListId;
    }
}