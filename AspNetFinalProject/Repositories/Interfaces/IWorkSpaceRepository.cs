
using AspNetFinalProject.Common;
using AspNetFinalProject.Entities;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Repositories.Interfaces;

public interface IWorkSpaceRepository
{
    Task<IEnumerable<WorkSpace>> GetUserWorkSpacesAsync(string userId);
    Task<PagedResult<WorkSpace>> GetUserWorkSpacesAsync(
        string userId,
        int page,
        int pageSize,
        WorkSpaceSearchField searchField = WorkSpaceSearchField.None,
        string? searchValue = null,
        WorkSpaceSortField sortField = WorkSpaceSortField.Title,
        SortDirection sortDirection = SortDirection.Ascending,
        CancellationToken ct = default);
    Task<WorkSpace?> GetByIdAsync(Guid id, bool withDeleted = false);
    Task AddAsync(WorkSpace workspace);
    Task DeleteAsync(WorkSpace workspace);
    Task SaveChangesAsync();
}