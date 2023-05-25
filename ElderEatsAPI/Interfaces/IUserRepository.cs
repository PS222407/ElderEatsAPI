using ElderEatsAPI.Models;

namespace ElderEatsAPI.Interfaces;

public interface IUserRepository
{
    public User Register(User user);

    public User Login(User user);
}