using System.ComponentModel.DataAnnotations.Schema;

namespace ElderEatsAPI.Models;

public class Product
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string ?Brand { get; set; }

    [Column("quantity_in_package")]
    public string ?QuantityInPackage { get; set; }

    public string ?Barcode { get; set; }

    public string? Image { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public List<AccountProduct>? AccountProducts { get; set; }
}
