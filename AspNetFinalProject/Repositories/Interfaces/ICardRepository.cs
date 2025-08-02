using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface ICardRepository
{
    Task<IEnumerable<Card>> GetCardsByListAsync(int boardListId, string userId);
    Task<Card?> GetByIdAsync(int id);
    Task AddAsync(Card card);
    Task DeleteAsync(Card card);
    Task SaveChangesAsync();
}