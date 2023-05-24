using ElderEatsAPI.Models;

namespace ElderEatsAPI.Interfaces;

public interface IProductRepository
{
    public List<Product> GetProducts();

    public Product GetProduct(int id);

    public bool StoreProduct(Product product);
}
