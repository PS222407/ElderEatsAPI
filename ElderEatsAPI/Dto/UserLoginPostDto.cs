using Microsoft.Build.Framework;

namespace ElderEatsAPI.Dto;

public class UserLoginPostDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}