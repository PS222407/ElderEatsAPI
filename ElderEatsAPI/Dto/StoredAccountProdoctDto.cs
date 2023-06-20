using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ElderEatsAPI.Enums;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Dto;

public class StoredAccountProdoctDto
{
    [Column(" account_id")] public long AccountId { get; set; }

    [Column("product_id")] public long ProductId { get; set; }

    [Column("id")] public int Id { get; set; }
    //expiration_date ran_out_at  created_at updated_at
    [Column("expiration_date")] public DateTime? ExpirationDate { get; set; }
    public ProductDto? Product { get; set; }

}