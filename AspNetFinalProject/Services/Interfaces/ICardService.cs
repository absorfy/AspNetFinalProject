using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface ICardService
{
    Task<IEnumerable<Card>> GetCardsByListAsync(int boardListId, string userId);
    Task<Card?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, string title, string? description, string? color, DateTime? deadline);
    Task<Card> CreateAsync(int boardListId, string title, string authorId, string? description = null, string? color = null, DateTime? deadline = null);
    Task<bool> DeleteAsync(int id, string deletedByUserId);
}