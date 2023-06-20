using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ElderEatsAPI.Enums;


namespace ElderEatsAPI.Dto;

public class ProductDto
{
    public long Id { get; set; }

    [Column("expiration_date")]
    public DateTime? ExpirationDate { get; set; }

    public long Connection_Id { get; set; }

    public string Name { get; set; }

    public string? Brand { get; set; }

    [Column("quantity_in_package")]
    public string? QuantityInPackage { get; set; }

    public string? Barcode { get; set; }

    public string? Image { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }


    //get: fn () => implode(' - ', array_filter([$this->name, $this->brand, $this->quantity_in_package])),
    public string full_name { get
        {
            string val = Name;
            if (!string.IsNullOrEmpty(Brand))
            {
                val += " - ";
                val += Brand;
            }
            if (!string.IsNullOrEmpty(QuantityInPackage))
            {
                val += " - ";
                val += QuantityInPackage;
            }
            return val;
        }
    }
}