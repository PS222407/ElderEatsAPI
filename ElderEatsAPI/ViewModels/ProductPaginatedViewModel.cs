namespace ElderEatsAPI.ViewModels;

public class PaginatedViewModel<T>
{
    public List<T> Data { get; }

    public Paginate Paging { get; }
    
    public PaginatedViewModel(List<T> data, int currentPage, int maxPages)
    {
        Data = data;
        Paging = new Paginate
        {
            CurrentPage = currentPage,
            NextPage = currentPage + 1 <= maxPages ? currentPage + 1 : null,
            PreviousPage = currentPage - 1 > 0 ? currentPage - 1 : null,
            MaxPages = maxPages,
        };
    }
}