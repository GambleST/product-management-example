namespace ProductInformationApi.Models.DTO;

public class GetManufacturerResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<ManufacturerProductDto> Products { get; set; } = [];

    public class ManufacturerProductDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
    }
}