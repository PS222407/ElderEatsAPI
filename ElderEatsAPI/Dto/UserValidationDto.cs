using ElderEatsAPI.Models;

namespace ElderEatsAPI.Dto;

public class UserValidationDto
{
    public User? User;

    public string? Reason { get; set; }
}