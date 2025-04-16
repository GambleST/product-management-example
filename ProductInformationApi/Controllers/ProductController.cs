using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductInformationApi.Contexts;
using ProductInformationApi.Models;

namespace ProductInformationApi.Controllers;


[ApiController]
[Route("products")]
public class ProductController(ProductInformationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await dbContext.Products.ToListAsync();
        return Ok(products);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductByIdAsync(Guid id)
    {
        var product = await dbContext.FindAsync<Product>(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto createProductDto)
    {
        // TODO
        // Check for duplicate names
        // 
        var createProductResult = await dbContext.AddAsync(new Product
        {
            Name = createProductDto.Name,
            ManufacturerId = createProductDto.ManufacturerId,
        });
        await dbContext.SaveChangesAsync();
        return Created($"/products/{createProductResult.Entity.Id}", createProductResult.Entity);
    }
}