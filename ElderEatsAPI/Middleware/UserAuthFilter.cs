using ElderEatsAPI.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ElderEatsAPI.Middleware;

public class UserAuthFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (Identity.ApiKeyMissing)
        {
            context.Result = new UnauthorizedObjectResult("API key is missing");
        }
        else if (Identity.User == null)
        {
            context.Result = new UnauthorizedObjectResult("Unauthorized action");
        }
    }
}