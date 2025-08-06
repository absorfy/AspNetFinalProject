using AspNetFinalProject.DTOs;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Services.Interfaces;

public interface IWorkSpaceService
{
    Task<IEnumerable<WorkSpace>> GetUserWorkSpacesAsync(string userId);
    Task<WorkSpace?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateWorkSpaceDto dto);
    Task<WorkSpace> CreateAsync(string authorId, CreateWorkSpaceDto dto);
    Task<bool> DeleteAsync(int id, string deletedByUserId);
}