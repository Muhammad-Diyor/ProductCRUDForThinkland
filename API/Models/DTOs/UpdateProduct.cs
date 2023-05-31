namespace API.Models.DTOs;

public class UpdateProduct
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public uint? Quantity { get; set; }
    public decimal? Price { get; set; }
    public IFormFile? Image { get; set; }
}
