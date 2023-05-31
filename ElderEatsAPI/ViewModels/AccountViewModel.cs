using Newtonsoft.Json;

namespace ElderEatsAPI.ViewModels;

// [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class AccountViewModel
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string Token { get; set; }

    public string? TemporaryToken { get; set; }

    public DateTime? TemporaryTokenExpiresAt { get; set; }

    public DateTime? NotificationLastSentAt { get; set; }

    public List<AccountUserViewModel> AccountUsers { get; set; }

    public List<AccountProductViewModel> AccountProducts { get; set; }

    // public bool ShouldSerializeAccountProducts()
    // {
    //     return AccountProducts != null && AccountProducts.Count > 0;
    // }
    //
    // public bool ShouldSerializeAccountUsers()
    // {
    //     return AccountUsers != null && AccountUsers.Count > 0;
    // }
}
