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

[Route("api/v2/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IProductRepository _productRepository;
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

    [HttpGet("TemporaryToken/{temporaryToken}/limted")]
    public IActionResult GetAccountByTemporaryTokenLimted(string temporaryToken)
    {
        AccountViewModel? accountViewModel = _mapper.Map<AccountViewModel>(_accountRepository.GetAccountByTemporaryToken(temporaryToken, true));

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

    [AuthFilter]
    [HttpGet("{accountID:int}/Users/{userId}/get")]
    public IActionResult GetAccountUserByIds(int accountID, int userId)
    {
        AccountUser? accountuser = _accountRepository.FindAccountUser(accountID, userId);

        if (accountuser == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(accountuser);
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

    [AccountAuthFilter("account")]
    [HttpPut("{accountId:int}/User/{userId:int}/Create")]
    public IActionResult CreateAccountUserConnection([FromRoute] int accountId, [FromRoute] int userId)
    {
        AccountUser? accountUser = _accountRepository.CreateAccountUser(accountId, userId);

        if (accountUser == null)
        {
            ModelState.AddModelError("", "Account User connection could not be made");

            return StatusCode(500, ModelState);
        }
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(accountUser);

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
    [HttpPut("{accountId:int}/Products/{productId:int}/Create")]
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

    [HttpPut("Products/{connectionID:int}/Ranout")]
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

    [HttpPut("{accountId:int}/FixedProducts/{productid:int}/RanOut")]
    public IActionResult AddFixedProduct([FromRoute] int accountId, [FromRoute] int productid)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        if (!_accountRepository.AccountExists(accountId) || !_accountRepository.ProductExists(productid))
    [AuthFilter]
    [HttpGet("{accountId:int}/Fixedproducts/")]
    public IActionResult GetFixedProducts([FromRoute] int accountId)
    {

        List<FixedProduct> fp = new List<FixedProduct>();

        fp = _accountRepository.GetFixedProducts(accountId);
        
        return Ok(fp);
    }

    [HttpPut("{accountId:int}/Fixedproducts/{productid:int}/Ranout")]
    public IActionResult AddFixedProduct([FromRoute] int accountId, [FromRoute] int productid)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        if(!_accountRepository.AccountExists(accountId) || !_productRepository.ProductExists(productid))
        {
            ModelState.AddModelError("", "Adding fixed product went wrong");

            return StatusCode(500, ModelState);
        }

        _accountRepository.StoreFixedProduct(accountId, productid);

        return NoContent();
    }
}
 [AuthFilter]
    [HttpGet("{accountId:int}/Fixedproducts/")]
    public IActionResult GetFixedProducts([FromRoute] int accountId)
    {

        List<FixedProduct> fp = new List<FixedProduct>();

        fp = _accountRepository.GetFixedProducts(accountId);
        
        return Ok(fp);
    }

    [HttpPut("{accountId:int}/Fixedproducts/{productid:int}/Ranout")]
    public IActionResult AddFixedProduct([FromRoute] int accountId, [FromRoute] int productid)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        if(!_accountRepository.AccountExists(accountId) || !_productRepository.ProductExists(productid))
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

    [AuthFilter]
    [HttpPut("{accountId:int}/Fixedproducts/Update")]
    public IActionResult UpdateFixedProducts([FromBody] Dictionary<int, int> data, [FromRoute] int accountId)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        if (!_accountRepository.UpdateActiveFixedProducts(data, accountId))
        {
            ModelState.AddModelError("", "products could not be updated");

            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

}
