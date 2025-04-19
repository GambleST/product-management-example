namespace ProductInformationApi.Models.DTO;

public class GetProductResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required Guid ManufacturerId { get; set; }
    public required string ManufacturerName { get; set; }
}