using AutoMapper;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Models;
using ElderEatsAPI.Requests;
using ElderEatsAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

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
    [HttpPost("{userId}/Accounts")]
    public IActionResult GetAccounts([FromRoute] int userId)
    {

        if(userId == null)
        {
            ModelState.AddModelError("", "no user");
            return BadRequest(ModelState);
        }
        List<Account> accounts = _userRepository.getConnectedAccounts(userId,false);

        return Ok(accounts);
        
    }

    [HttpPost("{userId}/Accounts/Active")]
    public IActionResult GetAccountsActive([FromRoute] int userId)
    {

        if (userId == null)
        {
            ModelState.AddModelError("", "no user");
            return BadRequest(ModelState);
        }
        List<Account>? accounts = _userRepository.getConnectedAccounts(userId);

        if(accounts == null)
        {
            return NotFound();
        }

        return Ok(accounts);

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