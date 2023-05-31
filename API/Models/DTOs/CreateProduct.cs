namespace API.Models.DTOs;

public class CreateProduct
{
    public string Name { get; set; }
    public string Description { get; set; }
    public uint Quantity { get; set; }
    public decimal Price { get; set; }
    public IFormFile Photo { get; set; }
}