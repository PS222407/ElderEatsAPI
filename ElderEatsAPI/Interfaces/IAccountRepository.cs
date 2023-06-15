using ElderEatsAPI.Dto;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Interfaces;

public interface IAccountRepository
{
    public Account? GetAccount(int id);

    public Account? GetAccountByToken(string token);

    public Account? GetAccountByTemporaryToken(string temporaryToken, bool NotExpiredOnly = false);

    public Account? GetAccountWithUsers(int id);

    public Account? GetAccountWithConnectedUsers(int id);

    public Account? GetAccountWithProcessingUsers(int id);

    public AccountUser? FindAccountUser(int accountId, int userId);

    public List<Product> GetAccountProducts(int id);

    public List<Product> GetAccountActiveProducts(int id);

    public Account? StoreAccount(Account account);

    public bool UpdateAccount(Account account);

    public bool UpdateAccountUserConnection(AccountUser accountUser);

    public bool AccountUserExists(int accountId, int userId);

    public bool DetachAccountUser(AccountUser accountUser);

    public AccountUser? CreateAccountUser(int accountId, int userId);

    List<AccountUser>? GetAccountsFromUser(int userId);

    public bool AccountExists(int id);

    public bool AddProductToAccount(AccountProduct accountProduct);

    public bool AccountProductExists(int accountProductId);

    public bool AccountProductRanOut(int accountProductId);

    public FixedProduct? StoreFixedProduct(int accountId, int productId);

    public List<FixedProduct>? GetFixedProducts(int accountId);

    public bool UpdateActiveFixedProducts(Dictionary<int, int> Data, int accountId);
}
