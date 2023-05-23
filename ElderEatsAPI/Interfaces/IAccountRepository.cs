using ElderEatsAPI.Dto;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Interfaces;

public interface IAccountRepository
{
    public Account GetAccount(int id);

    public List<Product> GetAccountProducts(int id);
}
