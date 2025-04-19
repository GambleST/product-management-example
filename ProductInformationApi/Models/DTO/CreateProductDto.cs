namespace ProductInformationApi.Models.DTO;

public class CreateProductDto
{
    public required string Name { get; set; }
    public required Guid ManufacturerId { get; set; }
}