using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductInformationApi.Contexts;
using ProductInformationApi.Models.DTO;
using ProductInformationApi.Models.Entities;

namespace ProductInformationApi.Controllers;

[ApiController]
[Route("manufacturers")]
public class ManufacturerController(ProductInformationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetManufacturers()
    {
        var manufacturers = await dbContext.Manufacturers
            .Select(m => new GetManufacturerResponseDto
            {
                Id = m.Id,
                Name = m.Name,
                Products = m.Products.Select(p => new GetManufacturerResponseDto.ManufacturerProductDto
                    {
                        Id = p.Id,
                        Name = p.Name
                    }
                ).ToList()
            }).ToListAsync();
        return Ok(manufacturers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetManufacturerById(Guid id)
    {
        var manufacturer = await dbContext.Manufacturers.Include(p => p.Products).Where(m => m.Id == id).Select(m =>
            new GetManufacturerResponseDto
            {
                Id = m.Id,
                Name = m.Name,
                Products = m.Products.Select(p => new GetManufacturerResponseDto.ManufacturerProductDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList()
            }).FirstOrDefaultAsync();
        ;

        if (manufacturer == null)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Manufacturer not found",
                Detail = $"No manufacturer exists with ID {id}",
                Instance = HttpContext.Request.Path
            });
        return Ok(manufacturer);
    }

    [HttpGet("{manufacturerId:guid}/products")]
    public async Task<IActionResult> GetProductsByManufacturerIdAsync(Guid manufacturerId)
    {
        var manufacturerIsValid = await dbContext.Manufacturers.AnyAsync(m => m.Id == manufacturerId);
        if (!manufacturerIsValid)
            return NotFound(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "manufacturerId", ["Manufacturer does not exist."] }
            }));

        var products = await dbContext.Products.Where(p => p.ManufacturerId == manufacturerId).Select(p =>
            new GetManufacturerResponseDto.ManufacturerProductDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToListAsync();
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> CreateManufacturerAsync([FromBody] CreateManufacturerDto createManufacturerDto)
    {
        var newManufacturer = new Manufacturer
        {
            Name = createManufacturerDto.Name
        };
        await dbContext.AddAsync(newManufacturer);
        await dbContext.SaveChangesAsync();

        var createdManufacturer = await dbContext.Manufacturers.Include(p => p.Products)
            .Where(m => m.Id == newManufacturer.Id).Select(m => new GetManufacturerResponseDto
            {
                Id = newManufacturer.Id,
                Name = newManufacturer.Name,
                Products = m.Products.Select(p => new GetManufacturerResponseDto.ManufacturerProductDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList()
            }).FirstOrDefaultAsync();

        return Created($"/manufacturers/{createdManufacturer!.Id}", createdManufacturer);
    }
}