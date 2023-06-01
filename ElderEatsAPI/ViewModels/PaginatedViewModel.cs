using ElderEatsAPI.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ElderEatsAPI.ViewModels;

public class PaginatedViewModel<T>
{
    public List<T> Data { get; }

    public Paginate Paginate { get; set; }

    public PaginatedViewModel(
        PaginateDto<T> paginateDto,
        int currentPage,
        int perPage,
        string? name,
        HttpContext httpContext,
        IUrlHelper url
    )
    {
        Data = paginateDto.Items;
        
        string protocol = httpContext.Request.IsHttps ? "https://" : "http://";
        string domainUrl = protocol + httpContext.Request.Host.Value;

        Paginate = new Paginate(
            currentPage,
            (int)Math.Ceiling(paginateDto.Count / (float)perPage),
            perPage,
            paginateDto.Items.Count,
            paginateDto.Count,
            domainUrl,
            url,
            name
        );
    }
}