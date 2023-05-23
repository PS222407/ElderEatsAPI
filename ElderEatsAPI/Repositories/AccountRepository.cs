using ElderEatsAPI.Data;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Repositories;

public class AccountRepository : IAccountRepository
{
    private DataContext _context;

    public AccountRepository(DataContext context)
    {
        _context = context;
    }

    public Account ?GetAccount(int id)
    {
        return _context.Accounts.Where(a => a.Id == id).FirstOrDefault();
    }

    public List<Product> GetAccountProducts(int id)
    {
        return _context.AccountProducts.Where(ap => ap.AccountId == id).Select(p => p.Product).ToList();
    }
}
