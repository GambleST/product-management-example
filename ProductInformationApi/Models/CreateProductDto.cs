namespace ProductInformationApi.Models;

public class CreateProductDto
{
        public string Name { get; set; }
        public Guid ManufacturerId { get; set; }
}