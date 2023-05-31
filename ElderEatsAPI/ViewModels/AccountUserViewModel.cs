using ElderEatsAPI.Enums;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.ViewModels;

public class AccountUserViewModel
{
    public int Status { get; set; } = (int)ConnectionStatus.InActive;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    
    public UserViewModel User { get; set; }
}