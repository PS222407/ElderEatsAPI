using ElderEatsAPI.Dto;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Interfaces;

public interface IUserRepository
{
    public UserValidationDto Register(User user);

    public UserValidationDto Login(User user);

    public User? GetUserByToken(string? token);

    public List<Account>? getConnectedAccounts(int user, bool ActiveAccountsOnly = true);
}