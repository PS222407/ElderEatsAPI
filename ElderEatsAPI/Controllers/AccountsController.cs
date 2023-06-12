using Microsoft.AspNetCore.Mvc;
using ElderEatsAPI.Models;
using ElderEatsAPI.Interfaces;
using AutoMapper;
using ElderEatsAPI.Auth;
using Microsoft.CodeAnalysis;
using ElderEatsAPI.Middleware;
using ElderEatsAPI.Requests;
using ElderEatsAPI.ViewModels;
using Microsoft.CodeAnalysis;
using ElderEatsAPI.Dto;

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

    [HttpGet("Token/{token}")]
    public IActionResult GetAccountByToken(string token)
    {
        AccountViewModel? accountViewModel = _mapper.Map<AccountViewModel>(_accountRepository.GetAccountByToken(token));

        if (accountViewModel == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(accountViewModel);
    }

    [HttpGet("TemporaryToken/{temporaryToken}")]
    public IActionResult GetAccountByTemporaryToken(string temporaryToken)
    {
        AccountViewModel? accountViewModel = _mapper.Map<AccountViewModel>(_accountRepository.GetAccountByTemporaryToken(temporaryToken));

        if (accountViewModel == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(accountViewModel);
    }

    [AuthFilter]
    [HttpGet("{id:int}/Users")]
    public IActionResult GetAccountUsers(int id)
    {
        AccountViewModel? accountViewModel = _mapper.Map<AccountViewModel>(_accountRepository.GetAccountWithUsers(id));

        if (accountViewModel == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(accountViewModel);
    }

    [AuthFilter]
    [HttpGet("{id:int}/Users/Connected")]
    public IActionResult GetAccountConnectedUsers(int id)
    {
        AccountViewModel? accountViewModel = _mapper.Map<AccountViewModel>(_accountRepository.GetAccountWithConnectedUsers(id));

        if (accountViewModel == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(accountViewModel);
    }

    [AuthFilter]
    [HttpGet("{id:int}/Users/InProcess")]
    public IActionResult GetAccountInProcessUsers(int id)
    {
        AccountViewModel? accountViewModel = _mapper.Map<AccountViewModel>(_accountRepository.GetAccountWithProcessingUsers(id));

        if (accountViewModel == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(accountViewModel);
    }

    [HttpPost]
    public IActionResult StoreAccount([FromBody] AccountStoreRequest accountDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        AccountViewModel? accountViewModel = _mapper.Map<AccountViewModel>(_accountRepository.StoreAccount(_mapper.Map<Account>(accountDto)));
        if (accountViewModel == null)
        {
            ModelState.AddModelError("", "Storing account went wrong");

            return StatusCode(500, ModelState);
        }

        return Ok(accountViewModel);
    }

    [AccountAuthFilter("account")]
    [HttpPut]
    public IActionResult UpdateAccount([FromBody] AccountStoreRequest accountDto)
    {
        int id = (int)Identity.Account!.Id;

        if (!_accountRepository.AccountExists(id))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        Account account = _mapper.Map<Account>(accountDto);
        account.Id = id;

        if (!_accountRepository.UpdateAccount(account))
        {
            ModelState.AddModelError("", "Updating account went wrong");

            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [AuthFilter]
    [HttpPut("{accountId:int}/User/{userId:int}")]
    public IActionResult UpdateAccountUserConnection([FromRoute] int accountId, [FromRoute] int userId, [FromBody] AccountUserUpdateRequest accountUserUpdateRequest)
    {
        AccountUser? accountUser = _accountRepository.FindAccountUser(accountId, userId);
        if (accountUser == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        accountUser.Status = (int)accountUserUpdateRequest.Status;
        if (!_accountRepository.UpdateAccountUserConnection(accountUser))
        {
            ModelState.AddModelError("", "Updating account user's connection status went wrong");

            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [AuthFilter]
    [HttpDelete("{accountId:int}/User/{userId:int}")]
    public IActionResult DetachAccountUser([FromRoute] int accountId, [FromRoute] int userId)
    {
        AccountUser? accountUser = _accountRepository.FindAccountUser(accountId, userId);

        if (accountUser == null)
        {
            return NotFound();
        }

        if (!_accountRepository.DetachAccountUser(accountUser))
        {
            ModelState.AddModelError("", "Detaching user went wrong");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    [HttpPut("{accountId:int}/products/{productId:int}/create")]
    public IActionResult CreateAccountProductConnection([FromRoute] int accountId, [FromRoute] int productId)
    {
        AccountProductDto accountProductDto = new AccountProductDto();

        accountProductDto.AccountId = accountId;
        accountProductDto.ProductId = productId;

        if (!ModelState.IsValid)
            return BadRequest();


        if (!_accountRepository.AddProductToAccount(_mapper.Map<AccountProduct>(accountProductDto)))
        {
            ModelState.AddModelError("", "Adding product went wrong");

            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpPut("products/{connectionID:int}/ranout")]
    public IActionResult AccountProductRanout([FromRoute] int connectionID)
    {

        if (!ModelState.IsValid)
            return BadRequest();


        if (!_accountRepository.AccountProductRanOut(connectionID))
        {
            ModelState.AddModelError("", "Adding product went wrong");

            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpPut("{accountId:int}/fixedproducts/{productid:int}/ranout")]
    public IActionResult AddFixedProduct([FromRoute] int accountId, [FromRoute] int productid)
    {

        if (!ModelState.IsValid)
            return BadRequest();
        if(!_accountRepository.AccountExists(accountId) || !_accountRepository.ProductExists(productid))
        {
            ModelState.AddModelError("", "Adding fixed product went wrong");

            return StatusCode(500, ModelState);
        }
        else
        {
            _accountRepository.StoreFixedProduct(accountId, productid);
        }



        return NoContent();
    }
}
