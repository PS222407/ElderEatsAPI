using Microsoft.AspNetCore.Mvc;
using ElderEatsAPI.Models;
using ElderEatsAPI.Interfaces;
using AutoMapper;
using ElderEatsAPI.Auth;
using Microsoft.CodeAnalysis;
using ElderEatsAPI.Middleware;
using ElderEatsAPI.Requests;
using ElderEatsAPI.ViewModels;
using ElderEatsAPI.Dto;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Protocol;

namespace ElderEatsAPI.Controllers;

[Route("api/v2/[controller]")]
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
    [HttpGet("{accountId:int}/FixedProducts/")]
    public IActionResult GetFixedProduct([FromRoute] int accountId)
    {
        List<FixedProduct>? fixedProducts = _accountRepository.GetFixedProducts(accountId);


        if (fixedProducts == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(fixedProducts);
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
    [AuthFilter]
    [HttpGet("{accountID:int}/Products")]
    public IActionResult GetProducts([FromRoute]int accountID)
    {
        List<AccountProduct> ap = _accountRepository.GetAccountActiveProducts(accountID);

        List<StoredAccountProdoctDto> sap = new List<StoredAccountProdoctDto>();

        foreach (AccountProduct product in ap)
        {
            StoredAccountProdoctDto tmp = _mapper.Map<StoredAccountProdoctDto>(product);

            tmp.Product = _mapper.Map<ProductDto>(product.Product);

            sap.Add(tmp);
        }
        sap = sap.OrderByDescending(s=>s.ExpirationDate.HasValue).ThenBy(s => s.ExpirationDate).ToList();

        if (ap == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(sap);
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

    [AuthFilter]
    [HttpPut("{accountId:int}/Fixedproducts/{productid:int}/Ranout")]
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

    [AuthFilter]
    [HttpPut("{accountId:int}/Fixedproducts/Update")]
    public IActionResult UpdateFixedProducts([FromRoute] int accountId, [FromBody] string data)
    {
        //data = data.Replace('"', '\'');

        Dictionary<int, int> values = JsonConvert.DeserializeObject<Dictionary<int, int>>(data);

        if (!_accountRepository.UpdateActiveFixedProducts(values, accountId))
        {
            ModelState.AddModelError("", "products could not be updated");
            return StatusCode(500, ModelState);
        }
        if (!ModelState.IsValid)
        return BadRequest();

        return NoContent();
    }
}
