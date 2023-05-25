namespace ElderEatsAPI.Dto;

public class ProductPostDto
{
    public string Name { get; set; }

    public string? Brand { get; set; }

    public string? QuantityInPackage { get; set; }

    public string? Barcode { get; set; }

    public string? Image { get; set; }
}