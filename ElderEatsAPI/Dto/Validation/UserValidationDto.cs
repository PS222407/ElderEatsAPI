using ElderEatsAPI.Models;

namespace ElderEatsAPI.Dto.Validation;

public class UserValidationDto
{
    public User? User;

    public string? Reason { get; set; }
}