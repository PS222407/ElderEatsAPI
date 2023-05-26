using ElderEatsAPI.Dto.Validation;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Interfaces;

public interface IUserRepository
{
    public UserValidationDto Register(User user);

    public UserValidationDto Login(User user);

    public User? AuthorizeWithToken(string? token);
}