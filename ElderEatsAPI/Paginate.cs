using Microsoft.AspNetCore.Mvc;

namespace ElderEatsAPI;

public class Paginate
{
    public int Total { get; set; }

    public int PerPage { get; set; }

    public int CurrentPage { get; set; }

    public int? LastPage { get; set; }

    public int? NextPage { get; set; }
    
    public int? PreviousPage { get; set; }
    
    public string FirstPageUrl { get; set; }

    public string LastPageUrl { get; set; }

    public string? NextPageUrl { get; set; }

    public string? PreviousPageUrl { get; set; }

    public string Path { get; set; }

    public int From { get; set; }

    public int To { get; set; }

    public Paginate(
        int currentPage,
        int lastPage,
        int perPage,
        int thisPageSize,
        int total,
        string domainUrl,
        IUrlHelper url,
        string? name
    )
    {
        CurrentPage = currentPage;
        NextPage = currentPage + 1 <= lastPage ? currentPage + 1 : null;
        PreviousPage = currentPage - 1 > 0 ? currentPage - 1 : null;
        LastPage = lastPage;
        PerPage = perPage;
        Total = total;
        Path = domainUrl;
        To = (currentPage - 1) * perPage + thisPageSize;
        From = (currentPage - 1) * perPage + 1;

        name ??= " ";
        
        FirstPageUrl = url.Link("SearchProductsFromAccountPaginated", new
        {
            name,
            take = perPage,
            page = 1
        })!;
        LastPageUrl = url.Link("SearchProductsFromAccountPaginated", new
        {
            name,
            take = perPage,
            page = lastPage
        })!;
        NextPageUrl = NextPage != null ? url.Link("SearchProductsFromAccountPaginated", new
        {
            name,
            take = perPage,
            page = NextPage
        }) : null;
        PreviousPageUrl = PreviousPage != null ? url.Link("SearchProductsFromAccountPaginated", new
        {
            name,
            take = perPage,
            page = PreviousPage
        }) : null;
    }
}