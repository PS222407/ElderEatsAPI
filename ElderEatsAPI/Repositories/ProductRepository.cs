using ElderEatsAPI.Auth;
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

    public ProductPaginateDto GetActiveProductsFromAccount(int skip, int take)
    {
        var query = _context.AccountProducts
            .Where(ap => ap.AccountId == Identity.Account!.Id && ap.RanOutAt > DateTime.Now)
            .OrderBy(ap => ap.ExpirationDate)
            .Select(ap => ap.Product);

        int count = query.Count();
        List<Product> products = query
            .Skip(skip)
            .Take(take)
            .ToList();

        return new ProductPaginateDto
        {
            Products = products,
            Count = count
        };
    }

    public Product? GetProduct(int id)
    {
        return _context.Products.FirstOrDefault(p => p.Id == id);
    }

    public Product? GetProductByBarcode(string barcode)
    {
        return _context.Products.FirstOrDefault(p => p.Barcode == barcode);
    }

    public ProductPaginateDto SearchProductsByNamePaginated(string? name, int skip, int take)
    {
        IQueryable<Product> query = _context.Products.Where(p => p.Name.Contains(name != null ? name.Trim() : ""));
        int count = query.Count();
        var list = query.OrderByDescending(product => product.Id)
            .Skip(skip)
            .Take(take)
            .ToList();

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

    public bool DeleteProductFromAccountById(int accountProductId)
    {
        AccountProduct? accountProduct = _context.AccountProducts.FirstOrDefault(ap => ap.Id == accountProductId);

        if (accountProduct == null)
        {
            return false;
        }

        _context.Remove(accountProduct);

        return Save();
    }

    public bool UpdateProductExpirationDateFromAccountById(int accountProductId, DateTime date)
    {
        AccountProduct? accountProduct = _context.AccountProducts.FirstOrDefault(ap => ap.Id == accountProductId);

        if (accountProduct == null)
        {
            return false;
        }

        accountProduct.ExpirationDate = date;
        
        return Save();
    }

    private bool Save()
    {
        return _context.SaveChanges() > 0;
    }
}