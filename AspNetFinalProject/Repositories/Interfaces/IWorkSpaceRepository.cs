
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IWorkSpaceRepository
{
    Task<IEnumerable<WorkSpace>> GetUserWorkSpacesAsync(string userId);
    Task<WorkSpace?> GetByIdAsync(Guid id);
    Task AddAsync(WorkSpace workspace);
    Task DeleteAsync(WorkSpace workspace);
    Task SaveChangesAsync();
}