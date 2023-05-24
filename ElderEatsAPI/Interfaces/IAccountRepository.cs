using ElderEatsAPI.Models;

namespace ElderEatsAPI.Interfaces;

public interface IAccountRepository
{
    public Account? GetAccount(int id);
    
    public Account? GetAccountByToken(string token);

    public List<User> GetAccountUsers(int id);

    public List<Product> GetAccountProducts(int id);

    public Account? StoreAccount(Account account);

    public bool UpdateAccount(Account account);

    public bool AccountExists(int id);
}
