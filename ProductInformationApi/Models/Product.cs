namespace ProductInformationApi.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid ManufacturerId { get; set; }
}