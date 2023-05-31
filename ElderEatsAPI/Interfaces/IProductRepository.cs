using ElderEatsAPI.Dto;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Interfaces;

public interface IProductRepository
{
    public List<Product> GetProducts();
    
    public List<Product> GetActiveProductsFromAccount();

    public Product? GetProduct(int id);

    public ProductPaginateDto SearchProductsByNamePaginated(string? name, int skip, int take);

    public Product? GetProductByBarcode(string barcode);

    public bool StoreProduct(Product product);
    
    public bool DeleteProductFromAccountById(int id);

    public bool UpdateProductExpirationDateFromAccountById(int id, DateTime date);
}
