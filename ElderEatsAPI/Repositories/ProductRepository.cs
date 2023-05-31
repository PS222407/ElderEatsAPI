using ElderEatsAPI.Data;
using ElderEatsAPI.Dto;
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

    public ProductPaginateDto SearchProductsByNamePaginated(string? name, int? skip, int? take)
    {
        IQueryable<Product> query = _context.Products.Where(p => p.Name.Contains(name != null ? name.Trim() : ""));
        int count = query.Count();
        
        query = query.OrderByDescending(product => product.Id);

        if (skip != null)
        {
            query = query.Skip(skip.Value);
        }
        if (take != null)
        {
            query = query.Take(take.Value);
        }

        var list = query.ToList();

        return new ProductPaginateDto
        {
            Products = list,
            Count = count,
        };
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
