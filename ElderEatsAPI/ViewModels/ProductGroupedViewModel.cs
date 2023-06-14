namespace ElderEatsAPI.ViewModels;

public class ProductGroupedViewModel
{
    public ProductViewModel Product { get; set; }
    
    public AccountProductViewModel AccountProduct { get; set; }

    public int Count { get; set; }
}