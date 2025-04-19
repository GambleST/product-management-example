namespace ProductInformationApi.Models.Entities;

public class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; }
}