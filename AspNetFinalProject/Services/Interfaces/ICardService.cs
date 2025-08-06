using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Services.Interfaces;

public interface ICardService
{
    Task<IEnumerable<Card>> GetCardsByListAsync(int boardListId, string userId);
    Task<Card?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateCardDto dto);
    Task<Card> CreateAsync(string authorId, CreateCardDto dto);
    Task<bool> DeleteAsync(int id, string deletedByUserId);
}