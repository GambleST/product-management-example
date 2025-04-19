namespace ProductInformationApi.Models.Entities;

public class Manufacturer
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<Product> Products { get; set; } = [];
}