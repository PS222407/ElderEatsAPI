using Newtonsoft.Json;

namespace ElderEatsAPI.Requests;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class AccountStoreRequest
{
    public string? Name { get; set; }

    public string? Token { get; set; }

    public string? TemporaryToken { get; set; }

    public DateTime? TemporaryTokenExpiresAt { get; set; }

    public DateTime? NotificationLastSentAt { get; set; }
}
