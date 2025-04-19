using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductInformationApi.Contexts;
using ProductInformationApi.Models.DTO;
using ProductInformationApi.Models.Entities;

namespace ProductInformationApi.Controllers;

[ApiController]
[Route("products")]
public class ProductController(ProductInformationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await dbContext.Products.Include(p => p.Manufacturer).Select(p => new GetProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            ManufacturerId = p.ManufacturerId,
            ManufacturerName = p.Manufacturer.Name
        }).ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductByIdAsync(Guid id)
    {
        var product = await dbContext.Products.Include(p => p.Manufacturer)
            .Select(p => new GetProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                ManufacturerId = p.ManufacturerId,
                ManufacturerName = p.Manufacturer.Name
            })
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto createProductDto)
    {
        var manufacturerIsValid = await dbContext.Manufacturers.AnyAsync(m => m.Id == createProductDto.ManufacturerId);
        if (!manufacturerIsValid)
            return NotFound(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "manufacturerId", ["Manufacturer does not exist."] }
            }));

        var newProduct = new Product
        {
            Name = createProductDto.Name,
            ManufacturerId = createProductDto.ManufacturerId
        };
        await dbContext.AddAsync(newProduct);
        await dbContext.SaveChangesAsync();

        var createdProduct = await dbContext.Products
            .Include(p => p.Manufacturer)
            .Where(p => p.Id == newProduct.Id)
            .Select(p => new GetProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                ManufacturerId = p.ManufacturerId,
                ManufacturerName = p.Manufacturer.Name
            })
            .FirstOrDefaultAsync();

        return Created($"/products/{createdProduct!.Id}", createdProduct);
    }
}