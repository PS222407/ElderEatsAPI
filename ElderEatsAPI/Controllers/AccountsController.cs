using Microsoft.AspNetCore.Mvc;
using ElderEatsAPI.Models;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Dto;
using AutoMapper;

namespace ElderEatsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public AccountsController(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    // [HttpGet("account/{id:int}")]
    // public IActionResult GetAccount(int id)
    // {
    //     AccountDto accountDto = _mapper.Map<AccountDto>(_accountRepository.GetAccount(id));
    //
    //     if (accountDto == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState);
    //     }
    //
    //     return Ok(accountDto);
    // }

    [HttpGet("token/{token}")]
    public IActionResult GetAccountByToken(string token)
    {
        AccountDto? accountDto = _mapper.Map<AccountDto>(_accountRepository.GetAccountByToken(token));

        if (accountDto == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(accountDto);
    }

    // [HttpGet("account/{id:int}/products")]
    // public IActionResult GetAccountProducts(int id)
    // {
    //     List<ProductDto> productsDto = _mapper.Map<List<ProductDto>>(_accountRepository.GetAccountProducts(id));
    //
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState);
    //     }
    //
    //     return Ok(productsDto);
    // }

    [HttpGet("{id:int}/users")]
    public IActionResult GetAccountUsers(int id)
    {
        List<UserDto> userDtos = _mapper.Map<List<UserDto>>(_accountRepository.GetAccountUsers(id));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(userDtos);
    }

    [HttpPost]
    public IActionResult StoreAccount([FromBody] AccountPostDto accountDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        AccountDto? account = _mapper.Map<AccountDto>(_accountRepository.StoreAccount(_mapper.Map<Account>(accountDto)));
        if (account == null)
        {
            ModelState.AddModelError("", "Storing account went wrong");
        
            return StatusCode(500, ModelState);
        }

        return Ok(account);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateAccount([FromRoute] int id, [FromBody] AccountPostDto accountDto)
    {
        if (!_accountRepository.AccountExists(id))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        Account account = _mapper.Map<Account>(accountDto);
        account.Id = id;

        if (!_accountRepository.UpdateAccount(account))
        {
            ModelState.AddModelError("", "Updating account went wrong");

            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
