using System.ComponentModel.DataAnnotations.Schema;

namespace ElderEatsAPI.Dto;

public class UserDto
{
    public long Id { get; set; }
    
    public string Name { get; set; }

    public string Email { get; set; }

    public DateTime? EmailVerifiedAt { get; set; }

    public string? RememberToken { get; set; }

    public DateTime? CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}