namespace API.Models.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public uint Quantity { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
}
