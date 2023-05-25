using AutoMapper;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElderEatsAPI.Controllers;

[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult Register(UserRegistrationDto userRegistrationDto)
    {
        User user = _mapper.Map<User>(userRegistrationDto);

        _userRepository.Register(user);

        return Ok(user);
    }
}