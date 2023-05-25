﻿using AutoMapper;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Dto.Validation;
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
    public IActionResult Register([FromBody] UserRegistrationPostDto userRegistrationPostDto)
    {
        User user = _mapper.Map<User>(userRegistrationPostDto);

        UserValidationDto userValidationDto = _userRepository.Register(user);
        if (userValidationDto.Reason != null)
        {
            ModelState.AddModelError("", userValidationDto.Reason);
            return BadRequest(ModelState);
        }

        return Ok(_mapper.Map<UserRegistrationDto>(user));
    }
}