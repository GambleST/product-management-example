namespace ProductInformationApi.Models;

public class Manufacturer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; } = [];
}