using ElderEatsAPI.Auth;
using ElderEatsAPI.Data;
using ElderEatsAPI.Models;
using ElderEatsAPI.Repositories;

namespace ElderEatsAPI.Middleware;

public class AuthMiddleware
{
    private const string TokenName = "x-api-key";

    private readonly RequestDelegate _next;
    
    private UserRepository _userRepository;
    
    private AccountRepository _accountRepository;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, DataContext context)
    {
        _userRepository = new UserRepository(context);
        _accountRepository = new AccountRepository(context);
        
        Identity.ApiKeyMissing = !httpContext.Request.Headers.TryGetValue(TokenName, out var apiKey);

        User? user = _userRepository.GetUserByToken(apiKey);
        Account? account = _accountRepository.GetAccountByToken(apiKey!);

        Identity.User = user;
        Identity.Account = account;

        await _next(httpContext);
    }
}