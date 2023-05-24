using System.ComponentModel.DataAnnotations.Schema;
using ElderEatsAPI.Enums;

namespace ElderEatsAPI.Models;

public class AccountUser
{
    public long Id { get; set; }

    [Column("account_id")]
    public long AccountId { get; set; }

    [Column("user_id")]
    public long UserId { get; set; }

    public int Status { get; set; } = (int)ConnectionStatus.InActive;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public Account Account { get; set; }

    public User User { get; set; }
}