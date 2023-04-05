namespace AngularGridPagination.Models;

public class PagedResult<T>
{
    public List<T> Results { get; set; } = default!;
    public int CurrentPage { get; set; }
    public int TotalItems { get; set; }
}