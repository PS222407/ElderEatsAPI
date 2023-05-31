using ElderEatsAPI.Data;
using ElderEatsAPI.Enums;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Models;
using Microsoft.EntityFrameworkCore;

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

    public Account? GetAccountByTemporaryToken(string temporaryToken)
    {
        return _context.Accounts.FirstOrDefault(a => a.TemporaryToken == temporaryToken);
    }

    public Account? GetAccountWithUsers(int id)
    {
        Account? account = _context.Accounts
            .Where(a => a.Id == id)
            .Include(a => a.AccountUsers)
            .ThenInclude(au => au.User)
            .FirstOrDefault();

        return account;
    }

    public Account? GetAccountWithConnectedUsers(int id)
    {
        Account? account = _context.Accounts
            .Where(a => a.Id == id)
            .Include(a => a.AccountUsers
                .Where(au => au.Status == (int)ConnectionStatus.Connected))
            .ThenInclude(au => au.User)
            .FirstOrDefault();

        return account;
    }

    public Account? GetAccountWithProcessingUsers(int id)
    {
        Account? account = _context.Accounts
            .Where(a => a.Id == id)
            .Include(a => a.AccountUsers
                .Where(au => au.Status == (int)ConnectionStatus.InProcess))
            .ThenInclude(au => au.User)
            .FirstOrDefault();

        return account;
    }

    public List<Product> GetAccountProducts(int id)
    {
        return _context.AccountProducts.Where(ap => ap.AccountId == id).Select(p => p.Product).ToList();
    }

    public List<Product> GetAccountActiveProducts(int id)
    {
        return _context.AccountProducts
            .Where(ap => ap.AccountId == id)
            .Where(ap => ap.ExpirationDate > DateTime.Now || ap.ExpirationDate == null)
            .Select(p => p.Product)
            .ToList();
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

    public bool UpdateAccountUserConnection(AccountUser accountUser)
    {
        accountUser.UpdatedAt = DateTime.Now;
        _context.Update(accountUser);

        return Save();
    }

    public bool AccountUserExists(int accountId, int userId)
    {
        return _context.AccountUsers.Any(au => au.UserId == userId && au.AccountId == accountId);
    }

    public AccountUser? FindAccountUser(int accountId, int userId)
    {
        return _context.AccountUsers.FirstOrDefault(au => au.UserId == userId && au.AccountId == accountId);
    }

    public bool DetachAccountUser(AccountUser accountUser)
    {
        _context.Remove(accountUser);

        return Save();
    }

    public bool AccountExists(int id)
    {
        return _context.Accounts.Any(a => a.Id == id);
    }

    public bool AddProductToAccount(AccountProduct accountProductDto)
    {
        accountProductDto.UpdatedAt = DateTime.Now;
        accountProductDto.CreatedAt = DateTime.Now;

        _context.Add(accountProductDto);

        return Save();
    }

    public bool AccountProductRanOut(int accountProductId)
    {
        AccountProduct? ap = _context.AccountProducts.FirstOrDefault(ap => ap.Id == accountProductId);

        if (ap == null)
        {
            return false;
        }

        ap.RanOutAt = DateTime.Now;
        ap.UpdatedAt = DateTime.Now;

        return Save();
    }

    public bool AccountProductExists(int accountProductId)
    {
        AccountProduct? ap = _context.AccountProducts.FirstOrDefault(ap => ap.Id == accountProductId);

        return ap == null;
    }

    public FixedProduct StoreFixedProduct(int accountId, int productId)
    {
        FixedProduct fixedProduct = new FixedProduct();

        fixedProduct.ProductId = productId;
        fixedProduct.AccountId = accountId;
        fixedProduct.isActive = true;
        fixedProduct.UpdatedAt = DateTime.Now;
        fixedProduct.CreatedAt = DateTime.Now;

        _context.FixedProducts.Add(fixedProduct);
        _context.SaveChanges();

        return fixedProduct;
    }

    public bool ProductExists(int id)
    {
        return _context.Products.Any(a => a.Id == id);
    }

    private bool Save()
    {
        return _context.SaveChanges() > 0;
    }
}