using ElderEatsAPI.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ElderEatsAPI.Dto;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class AccountDto
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string Token { get; set; }

    public string? TemporaryToken { get; set; }

    public DateTime? TemporaryTokenExpiresAt { get; set; }

    public DateTime? NotificationLastSentAt { get; set; }

/*    public List<AccountProduct> AccountProducts { get; set; }

    public bool ShouldSerializeAccountProducts()
    {
        return AccountProducts != null && AccountProducts.Count > 0;
    }*/
}
