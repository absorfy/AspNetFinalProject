using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Services.Interfaces;

public interface IWorkSpaceService
{
    Task<IEnumerable<WorkSpace>> GetUserWorkSpacesAsync(string userId);
    Task<WorkSpace?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, string title, string? description, WorkSpaceVisibility visibility);
    Task<WorkSpace> CreateAsync(string title, string authorId, string? description = null);
    Task<bool> DeleteAsync(int id, string deletedByUserId);
}