using System.ComponentModel.DataAnnotations.Schema;

namespace ElderEatsAPI.Models;

public class FixedProduct
{
    public long Id { get; set; }

    [Column("account_id")]
    public long AccountId { get; set; }

    [Column("product_id")]
    public long ProductId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_active")]
    public bool isActive { get; set; }

    public Account Account { get; set; }

    public Product Product { get; set; }
}
