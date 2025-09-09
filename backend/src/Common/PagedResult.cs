namespace AspNetFinalProject.Common;

public sealed class PagedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalCount { get; init; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
    
    
    public PagedResult<TResult> Map<TResult>(
        Func<T, TResult> selector)
    {
        return new PagedResult<TResult>
        {
            Items = Items.Select(selector).ToList(),
            Page = Page,
            PageSize = PageSize,
            TotalCount = TotalCount
        };
    }
    
    
    public async Task<PagedResult<TResult>> MapAsync<TResult>(
        Func<T, Task<TResult>> selector)
    {
        var items = await Task.WhenAll(Items.Select(selector));

        return new PagedResult<TResult>
        {
            Items = items,
            Page = Page,
            PageSize = PageSize,
            TotalCount = TotalCount
        };
    }
}