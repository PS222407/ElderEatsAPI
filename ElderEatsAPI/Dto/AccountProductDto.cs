using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ElderEatsAPI.Enums;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Dto;

public class AccountProductDto
{
    [Column(" account_id")] public long AccountId { get; set; }

    [Column("product_id")] public long ProductId { get; set; }

    [Column("created_at")] public DateTime? CreatedAt { get; set; }

    [Column("updated_at")] public DateTime? UpdatedAt { get; set; }

}