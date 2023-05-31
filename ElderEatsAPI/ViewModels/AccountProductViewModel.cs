namespace ElderEatsAPI.ViewModels;

public class AccountProductViewModel
{
    public DateTime? ExpirationDate { get; set; }

    public DateTime? RanOutAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}