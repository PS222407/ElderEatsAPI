using ElderEatsAPI.Data;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Enums;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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

    public Account? GetAccountByTemporaryToken(string temporaryToken, bool NotExpiredOnly = false)
    {
        if (!NotExpiredOnly)
        {
            return _context.Accounts.FirstOrDefault(a => a.TemporaryToken == temporaryToken);
        }
        else
        {
            return _context.Accounts.FirstOrDefault(a => a.TemporaryToken == temporaryToken && a.TemporaryTokenExpiresAt > DateTime.Now);
        }

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
            .Where(ap => ap.RanOutAt > DateTime.Now || ap.RanOutAt == null)
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
    public List<AccountUser>? GetAccountsFromUser(int userId)
    {
        return _context.AccountUsers.Where(au => au.UserId == userId).ToList();
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

    public bool AddProductToAccount(AccountProduct accountProduct)
    {
        accountProduct.UpdatedAt = DateTime.Now;
        accountProduct.CreatedAt = DateTime.Now;

        _context.Add(accountProduct);

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

    private bool Save()
    {
        return _context.SaveChanges() > 0;
    }

    public List<FixedProduct>? GetFixedProducts(int accountId)
    {
        List<FixedProduct> p = new List<FixedProduct>();

        p = _context.FixedProducts.Where(p => p.AccountId == accountId && p.isActive == true).ToList();

        return p;
    }
    public bool IsFixedProductConnectedToAccount(int accountId, FixedProduct fixedProduct)
    {
        FixedProduct? TestProduct = _context.FixedProducts.Where(fp => fp.AccountId == accountId && fp.Id == fixedProduct.Id).FirstOrDefault();
        if (TestProduct != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool UpdateActiveFixedProducts(Dictionary<int, int> Data, int accountId)
    {
        bool returnVal = true;

        foreach (int key in Data.Keys)
        {
            FixedProduct? fixedproduct = _context.FixedProducts.Where(fp => fp.Id == key).FirstOrDefault();
            if (fixedproduct != null)
            {
                if (IsFixedProductConnectedToAccount(accountId, fixedproduct))
                {
                    fixedproduct.isActive = Convert.ToBoolean(Data[key]);
                    fixedproduct.UpdatedAt = DateTime.Now;
                }
                else
                {
                    returnVal = false;
                }
            }
            else
            {
                returnVal = false;
            }
        }
        _context.SaveChanges();

        return returnVal;
    }


    public AccountUser? CreateAccountUser(int accountId, int userId)
    {
        IUserRepository u = new UserRepository(_context);

        if (AccountExists(accountId) && u.FindUserById(userId) != null)
        {

            AccountUser? accountUser = new AccountUser();


            if (AccountUserExists(accountId, userId))
            {
                accountUser = _context.AccountUsers.Where(au => au.AccountId == accountId && au.UserId == userId).FirstOrDefault();
                accountUser.UpdatedAt = DateTime.Now;
                if (accountUser != null)
                {
                    accountUser.Status = 1;
                    Save();

                    return accountUser;
                }
            }
            else
            {         
                accountUser.AccountId = accountId;
                accountUser.UserId = userId;
                accountUser.Status = 1;
                accountUser.CreatedAt = DateTime.Now;
                accountUser.UpdatedAt = DateTime.Now;
                _context.AccountUsers.Add(accountUser);

                Save();

                accountUser = _context.AccountUsers.Where(au => au.AccountId == accountId && au.UserId == userId).FirstOrDefault();
                if (accountUser != null)
                {
                    return accountUser;
                }
            }
        }
        return null;
    }
}
