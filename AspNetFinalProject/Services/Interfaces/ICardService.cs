using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface ICardService
{
    Task<IEnumerable<Card>> GetCardsByListAsync(Guid boardListId, string userId);
    Task<Card?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, UpdateCardDto dto, string updateByUserId);
    Task<Card> CreateAsync(string authorId, CreateCardDto dto);
    Task<bool> DeleteAsync(Guid id, string deletedByUserId, bool notify = true);
    Task MoveCard(Guid cardId, Guid newListId, int orderIndex, string movedByUserId); 
}