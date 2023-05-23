using System.ComponentModel.DataAnnotations.Schema;

namespace ElderEatsAPI.Models;

public class AccountProduct
{
    public long Id { get; set; }

    [Column("account_id")]
    public long AccountId { get; set; }

    [Column("product_id")]
    public long ProductId { get; set; }

    [Column("expiration_date")]
    public DateTime ?ExpirationDate { get; set; }

    [Column("ran_out_at")]
    public DateTime ?RanOutAt { get; set; }

    public Account Account { get; set; }

    public Product Product { get; set; }
}
