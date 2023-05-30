using ElderEatsAPI.Dto;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Interfaces;

public interface IAccountRepository
{
    public Account? GetAccount(int id);
    
    public Account? GetAccountByToken(string token);

    public Account? GetAccountWithUsers(int id);

    public List<Product> GetAccountProducts(int id);

    public Account? StoreAccount(Account account);

    public bool UpdateAccount(Account account);

    public bool UpdateAccountUserConnection(AccountUser accountUser);
    
    public bool AccountUserExists(int accountId, int userId);

    public AccountUser? FindAccountUser(int accountId, int userId);

    public bool AccountExists(int id);

    public bool AddProductToAccount(AccountProduct accountProduct);

    public bool AccountProductExists(int AccountProductID);
    public bool AccountProductRanOut(int AccountProductID);

    public FixedProduct? StoreFixedProduct(int AccountID, int ProductID);

    public bool ProductExists(int productID);
}
