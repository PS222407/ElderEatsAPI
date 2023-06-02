using ElderEatsAPI.Models;

namespace ElderEatsAPI.Dto;

public class PaginateDto<T>
{
    public List<T> Items { get; set; }
    
    public int Count { get; set; }
}