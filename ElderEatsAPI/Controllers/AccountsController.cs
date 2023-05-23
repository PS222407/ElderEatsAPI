using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElderEatsAPI.Models;
using ElderEatsAPI.Data;
using ElderEatsAPI.Repositories;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Dto;
using AutoMapper;

namespace ElderEatsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private IAccountRepository _accountRepository;
    private IMapper _mapper;

    public AccountsController(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    [HttpGet("account/{id}")]
    public IActionResult GetAccount(int id)
    {
        AccountDto accounts = _mapper.Map<AccountDto>(_accountRepository.GetAccount(id));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(accounts);
    }

    [HttpGet("account/{id}/products")]
    public IActionResult GetAccountProducts(int id)
    {
        List<ProductDto> products = _mapper.Map<List<ProductDto>>(_accountRepository.GetAccountProducts(id));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(products);
    }
}
