using System.Text.Json;
using ElderEatsAPI.Auth;
using ElderEatsAPI.Interfaces;
using ElderEatsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ElderEatsAPI.Middleware;

public class AuthUserMiddleware : IAuthorizationFilter
{
    private const string TokenName = "token";
    
    private readonly IUserRepository _userRepository;
    
    public AuthUserMiddleware(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // var requestBody = string.Empty;
        //
        // using (var reader = new StreamReader(context.HttpContext.Request.Body))
        // {
        //     requestBody = reader.ReadToEnd();
        //     context.HttpContext.Request.Body.Position = 0;
        // }
        //
        // var jsonDocument = JsonDocument.Parse(requestBody);
        //
        // if (!jsonDocument.RootElement.TryGetProperty(TokenName, out var extractedToken))
        // {
        //     context.Result = new UnauthorizedObjectResult("Token is missing");
        //     return;
        // }
        //
        // var tokenValue = extractedToken.ToString();
        // System.Diagnostics.Debug.Write(tokenValue);
        //
        // context.Result = new UnauthorizedObjectResult(tokenValue);
        // return;

        var originalBody = context.HttpContext.Request.Body;
        var requestBody = Task.Run(async () => await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync()).GetAwaiter().GetResult();
        context.HttpContext.Request.Body = originalBody;
        // var jsonDocument = JsonDocument.Parse(requestBody);
        
        // if (!jsonDocument.RootElement.TryGetProperty(TokenName, out var extractedToken))
        // {
        //     context.Result = new UnauthorizedObjectResult("Token is missing");
        //     return;
        // }
        
        // User? user = _userRepository.AuthorizeWithToken(extractedToken.ToString());
        // if (user == null)
        // {
        //     context.Result = new UnauthorizedObjectResult("Invalid token");
        //     return;
        // }
        //
        // Identity.User = user;
        // return;
    }
}