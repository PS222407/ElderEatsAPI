using ElderEatsAPI.Models;

namespace ElderEatsAPI.Dto;

public class ProductGroupedDto
{
    public Product Product { get; set; }
    
    public AccountProduct AccountProduct { get; set; }

    public int Count { get; set; }
}