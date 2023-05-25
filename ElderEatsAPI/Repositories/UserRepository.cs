using ElderEatsAPI.Data;
using ElderEatsAPI.Dto.Validation;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }
    
    public UserValidationDto Register(User user)
    {
        UserValidationDto userValidationDto = new UserValidationDto();
        if (UserExistsByEmail(user.Email))
        {
            userValidationDto.Reason = "Email is already taken";
            return userValidationDto;
        }
        
        user.Token = Guid.NewGuid().ToString();
        _context.Users.Add(user);

        if (!Save())
        {
            userValidationDto.Reason = "Something went wrong while registering user";
            return userValidationDto;
        }

        userValidationDto.User = user;
        return userValidationDto;
    }

    public User? Login(User user)
    {
        throw new NotImplementedException();
    }

    private bool UserExistsByEmail(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }

    private bool Save()
    {
        return _context.SaveChanges() > 0;
    }
}