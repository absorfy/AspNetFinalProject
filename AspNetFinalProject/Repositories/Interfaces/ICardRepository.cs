using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface ICardRepository
{
    Task<IEnumerable<Card>> GetCardsByListAsync(Guid boardListId, string userId);
    Task<Card?> GetByIdAsync(Guid id, bool withDeleted = false);
    Task AddAsync(Card card);
    Task DeleteAsync(Card card);
    Task SaveChangesAsync();
    Task MoveCard(Guid cardId, Guid newListId, int orderIndex);
}