using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductInformationApi.Contexts;
using ProductInformationApi.Models;

namespace ProductInformationApi.Controllers;

[ApiController]
[Route("manufacturer")]
public class ManufacturerController(ProductInformationDbContext dbContext) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetManufacturerById(Guid manufacturerId)
    {
        var manufacturer = await dbContext.FindAsync<Manufacturer>(manufacturerId);
        if (manufacturer == null)
        {
            return NotFound();
        }
        return Ok(manufacturer);
    }
    
    [HttpGet("{manufacturerId:guid}/products")]
    public async Task<IActionResult> GetProductsByManufacturerIdAsync(Guid manufacturerId)
    {
        var products = await dbContext.Products.Where(p => p.ManufacturerId == manufacturerId).ToListAsync();
        return Ok(products);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateManufacturerAsync([FromBody] CreateManufacturerDto createManufacturerDto)
    {
        // TODO
        // Check for duplicate names
        // Do we need to allow submitting a list of products under them?
        var createManufacturerResult = await dbContext.AddAsync(new Manufacturer
        {
            Name = createManufacturerDto.Name,
        });
        await dbContext.SaveChangesAsync();
        return Created($"/manufacturers/{createManufacturerResult.Entity.Id}", createManufacturerResult.Entity);
    }
}