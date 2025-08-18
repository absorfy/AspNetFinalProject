using AspNetFinalProject.Common;
using AspNetFinalProject.Entities;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IUserActionLogRepository
{
    Task AddAsync(UserActionLog log);
    Task SaveChangesAsync();
    Task<IEnumerable<UserActionLog>> GetByUserIdAsync(string userId);
    Task<PagedResult<UserActionLog>> GetByUserIdAsync(string userId, PagedRequest request);
}