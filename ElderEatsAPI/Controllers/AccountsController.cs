using Microsoft.AspNetCore.Mvc;
using ElderEatsAPI.Models;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Dto;
using AutoMapper;
using ElderEatsAPI.Middleware;
using ElderEatsAPI.ViewModels;

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

    [HttpGet("Token/{token}")]
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

    [HttpGet("TemporaryToken/{temporaryToken}")]
    public IActionResult GetAccountByTemporaryToken(string temporaryToken)
    {
        AccountDto? accountDto = _mapper.Map<AccountDto>(_accountRepository.GetAccountByTemporaryToken(temporaryToken));

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

    [HttpGet("account/{id:int}/Products/Active")]
    public IActionResult GetAccountActiveProducts(int id)
    {
        List<ProductViewModel> productsDto = _mapper.Map<List<ProductViewModel>>(_accountRepository.GetAccountActiveProducts(id));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(productsDto);
    }

    [AccountAuthFilter("employee")]
    [HttpGet("{id:int}/Users")]
    public IActionResult GetAccountUsers(int id)
    {
        Account? account = _accountRepository.GetAccountWithUsers(id);

        if (account == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(account);
    }

    [HttpGet("{id:int}/Users/Connected")]
    public IActionResult GetAccountConnectedUsers(int id)
    {
        Account? account = _accountRepository.GetAccountWithConnectedUsers(id);

        if (account == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(account);
    }

    [HttpGet("{id:int}/Users/InProcess")]
    public IActionResult GetAccountInProcessUsers(int id)
    {
        Account? account = _accountRepository.GetAccountWithProcessingUsers(id);

        if (account == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(account);
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

    [HttpPut("{accountId:int}/User/{userId:int}")]
    public IActionResult UpdateAccountUserConnection([FromRoute] int accountId, [FromRoute] int userId, [FromBody] AccountUserDto accountUserDto)
    {
        AccountUser? accountUser = _accountRepository.FindAccountUser(accountId, userId);
        if (accountUser == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        accountUser.Status = (int)accountUserDto.Status;
        if (!_accountRepository.UpdateAccountUserConnection(accountUser))
        {
            ModelState.AddModelError("", "Updating account user's connection status went wrong");

            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{accountId}/User/{userId}")]
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
        if (!_accountRepository.AccountExists(accountId) || !_accountRepository.ProductExists(productid))
        {
            ModelState.AddModelError("", "Adding fixed product went wrong");

            return StatusCode(500, ModelState);
        }

        _accountRepository.StoreFixedProduct(accountId, productid);

        return NoContent();
    }
}