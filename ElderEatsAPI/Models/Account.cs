using System.ComponentModel.DataAnnotations.Schema;

namespace ElderEatsAPI.Models;

public class Account
{
    public long Id { get; set; }

    public string ?Name { get; set; }

    public string Token { get; set; }

    [Column("temporary_token")]
    public string ?TemporaryToken { get; set; }

    [Column("temporary_token_expires_at")]
    public DateTime ?TemporaryTokenExpiresAt { get; set; }

    [Column("notification_last_sent_at")]
    public DateTime ?NotificationLastSentAt { get; set; }
    
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public List<AccountProduct> AccountProducts { get; set; }
    
    public List<AccountUser> AccountUsers { get; set; }
}
