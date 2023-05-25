using ElderEatsAPI.Dto.Validation;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Interfaces;

public interface IUserRepository
{
    public UserValidationDto Register(User user);

    public User? Login(User user);
}