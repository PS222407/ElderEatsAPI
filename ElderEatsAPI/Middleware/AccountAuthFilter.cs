using ElderEatsAPI.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ElderEatsAPI.Middleware;

public class AccountAuthFilter : Attribute, IAuthorizationFilter
{
    private readonly string _role;

    public AccountAuthFilter(string role)
    {
        _role = role;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //TODO: add role to user
        
        if (Identity.ApiKeyMissing)
        {
            context.Result = new UnauthorizedObjectResult("API key is missing");
        }
        else if (Identity.Account == null)
        {
            context.Result = new UnauthorizedObjectResult("Unauthorized action");
        }
    }
}