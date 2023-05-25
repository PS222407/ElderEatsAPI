using ElderEatsAPI.Data;
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
    
    public User Register(User user)
    {
        _context.Users.Add(user);

        return user;
    }

    public User Login(User user)
    {
        throw new NotImplementedException();
    }
}