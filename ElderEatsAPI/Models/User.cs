using System.ComponentModel.DataAnnotations.Schema;

namespace ElderEatsAPI.Models;

public class User
{
    public long Id { get; set; }
    
    public string Name { get; set; }

    public string Email { get; set; }

    [Column("email_verified_at")]
    public DateTime? EmailVerifiedAt { get; set; }

    [Column("remember_token")]
    public string? RememberToken { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public List<AccountUser> AccountUsers { get; set; }
}