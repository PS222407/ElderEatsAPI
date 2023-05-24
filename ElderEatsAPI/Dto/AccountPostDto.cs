using Newtonsoft.Json;

namespace ElderEatsAPI.Dto;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class AccountPostDto
{
    public string? Name { get; set; }

    public string? Token { get; set; }

    public string? TemporaryToken { get; set; }

    public DateTime? TemporaryTokenExpiresAt { get; set; }

    public DateTime? NotificationLastSentAt { get; set; }
}
