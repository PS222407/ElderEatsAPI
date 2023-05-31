using AutoMapper;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Models;
using ElderEatsAPI.Requests;
using ElderEatsAPI.ViewModels;
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

    [HttpPost("Register")]
    public IActionResult Register([FromBody] UserRegistrationRequest userRegistrationRequest)
    {
        User user = _mapper.Map<User>(userRegistrationRequest);

        UserValidationDto userValidationDto = _userRepository.Register(user);
        if (userValidationDto.Reason != null)
        {
            ModelState.AddModelError("", userValidationDto.Reason);
            return BadRequest(ModelState);
        }

        return Ok(_mapper.Map<UserRegistrationViewModel>(userValidationDto.User));
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] UserLoginRequest userLoginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        User user = _mapper.Map<User>(userLoginRequest);

        UserValidationDto userValidationDto = _userRepository.Login(user);
        if (userValidationDto.Reason != null)
        {
            ModelState.AddModelError("", userValidationDto.Reason);
            return BadRequest(ModelState);
        }

        return Ok(_mapper.Map<UserRegistrationViewModel>(userValidationDto.User));
    }
}