using AutoMapper;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Dto.Validation;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Middleware;
using ElderEatsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElderEatsAPI.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(AuthUserMiddleware))]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpPost("Register")]
    public IActionResult Register([FromBody] UserRegistrationPostDto userRegistrationPostDto)
    {
        User user = _mapper.Map<User>(userRegistrationPostDto);

        UserValidationDto userValidationDto = _userRepository.Register(user);
        if (userValidationDto.Reason != null)
        {
            ModelState.AddModelError("", userValidationDto.Reason);
            return BadRequest(ModelState);
        }

        return Ok(_mapper.Map<UserRegistrationDto>(userValidationDto.User));
    }
    
    [HttpPost("Login")]
    public IActionResult Login([FromBody] UserLoginPostDto userLoginPostDto)
    {
        User user = _mapper.Map<User>(userLoginPostDto);

        UserValidationDto userValidationDto = _userRepository.Login(user);
        if (userValidationDto.Reason != null)
        {
            ModelState.AddModelError("", userValidationDto.Reason);
            return BadRequest(ModelState);
        }

        return Ok(_mapper.Map<UserRegistrationDto>(userValidationDto.User));
    }
}