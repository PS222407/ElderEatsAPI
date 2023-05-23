using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

    public List<AccountProduct> AccountProducts { get; set; }
}
