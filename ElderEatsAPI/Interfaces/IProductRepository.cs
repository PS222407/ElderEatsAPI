using ElderEatsAPI.Dto;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Interfaces;

public interface IProductRepository
{
    public List<Product> GetProducts();

    public PaginateDto<Product> GetActiveProductsFromAccount(int skip, int take);

    public PaginateDto<ProductGroupedDto> GetProductsFromAccountPaginated(string name, int skip, int take);

    public Product? GetProduct(int id);

    public PaginateDto<Product> SearchProductsByNamePaginated(string? name, int skip, int take);

    public Product? GetProductByBarcode(string barcode);

    public bool StoreProduct(Product product);

    public bool DeleteProductFromAccountById(int accountProductId);

    public bool UpdateProductExpirationDateFromAccountById(int id, DateTime date);
}