using ElderEatsAPI.Data;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _context;

    public ProductRepository(DataContext context)
    {
        _context = context;
    }

    public List<Product> GetProducts()
    {
        return _context.Products.ToList();
    }

    public Product? GetProduct(int id)
    {
        return _context.Products.FirstOrDefault(p => p.Id == id);
    }

    public bool StoreProduct(Product product)
    {
        product.CreatedAt = DateTime.Now;
        product.UpdatedAt = DateTime.Now;
        _context.Add(product);

        return Save();
    }

    private bool Save()
    {
        return _context.SaveChanges() > 0;
    }
}
