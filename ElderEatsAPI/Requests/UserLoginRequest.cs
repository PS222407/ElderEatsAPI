using Microsoft.Build.Framework;

namespace ElderEatsAPI.Requests;

public class UserLoginRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}