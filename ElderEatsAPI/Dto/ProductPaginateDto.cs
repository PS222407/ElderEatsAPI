using ElderEatsAPI.Models;

namespace ElderEatsAPI.Dto;

public class ProductPaginateDto
{
    public List<Product> Products { get; set; }
    
    public int Count { get; set; }
}