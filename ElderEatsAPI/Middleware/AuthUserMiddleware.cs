using System.Security.Claims;
using ElderEatsAPI.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ElderEatsAPI.Middleware;

public class AuthUserMiddleware : IAsyncAuthorizationFilter
{
    private const string TokenName = "token";
    
    private readonly IUserRepository _userRepository;
    
    public AuthUserMiddleware(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Request.Headers.TryGetValue("x-api-key", out var apiKey))
        {
            var claims = new List<Claim>
            {
                new Claim("email", "jens@ramakers.nl"),
                new Claim("DeIpad", "A4"),
            };

            var identity = new ClaimsIdentity(claims, "MyAuthenticationScheme");

            var principal = new ClaimsPrincipal(identity);

            await context.HttpContext.SignInAsync(principal);
        }
        else
        {
            context.Result = new UnauthorizedObjectResult("invalid token");
        }
    }
}