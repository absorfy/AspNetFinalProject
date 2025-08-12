using Microsoft.EntityFrameworkCore;

namespace AspNetFinalProject.Common;

public static class QueryableExtensions
{
    /// <summary>
    /// Перетворює будь-який IQueryable у сторінку з елементами.
    /// Обмежує page >= 1 і pageSize в діапазоні [1, 100].
    /// </summary>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 1 : pageSize;
        if (pageSize > 100) pageSize = 100;

        var total = await query.CountAsync(ct);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = total
        };
    }
}