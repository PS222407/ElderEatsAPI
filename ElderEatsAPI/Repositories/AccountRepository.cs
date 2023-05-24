using ElderEatsAPI.Data;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly DataContext _context;

    public AccountRepository(DataContext context)
    {
        _context = context;
    }

    public Account? GetAccount(int id)
    {
        return _context.Accounts.FirstOrDefault(a => a.Id == id);
    }

    public Account? GetAccountByToken(string token)
    {
        return _context.Accounts.FirstOrDefault(a => a.Token == token);
    }

    public List<User> GetAccountUsers(int id)
    {
        return _context.AccountUsers.Where(au => au.AccountId == id).Select(u => u.User).ToList();
    }

    public List<Product> GetAccountProducts(int id)
    {
        return _context.AccountProducts.Where(ap => ap.AccountId == id).Select(p => p.Product).ToList();
    }

    public Account? StoreAccount(Account account)
    {
        account.CreatedAt = DateTime.Now;
        account.UpdatedAt = DateTime.Now;
        _context.Add(account);

        return Save() ? account : null;
    }

    public bool UpdateAccount(Account account)
    {
        account.UpdatedAt = DateTime.Now;
        _context.Update(account);

        return Save();
    }

    public bool AccountExists(int id)
    {
        return _context.Accounts.Any(a => a.Id == id);
    }

    private bool Save()
    {
        return _context.SaveChanges() > 0;
    }
}
