using Microsoft.EntityFrameworkCore;
using ProductInformationApi.Models;

namespace ProductInformationApi.Contexts;

public class ProductInformationDbContext(DbContextOptions<ProductInformationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Manufacturer IDs
        var appleId = Guid.Parse("5309b913-7cb5-48b4-b32d-2188da5ef21a");
        var sonyId = Guid.Parse("c0db3ed1-80dc-491f-812a-84bdd749ba03");
        var dellId = Guid.Parse("de29706e-27e0-4765-aeea-835cc605b4f3");

        // Product IDs
        var iphoneId = Guid.Parse("4f3c2bd3-d55f-4d76-b31c-d49e99d6c721");
        var macbookId = Guid.Parse("b62b5be6-d52b-4e70-bc3d-20d4a8e3e8dc");
        var ps5Id = Guid.Parse("e9171371-3aeb-4420-8ba0-91ec3b54193b");
        var headphonesId = Guid.Parse("5c79e189-2e8c-414e-bdf1-d3b3de2bb226");
        var xpsId = Guid.Parse("fa3c2285-74f3-4c71-a2d3-3c8b80829ac7");
        var alienwareId = Guid.Parse("0d71b234-d672-4e8b-a7f5-13d55cd3b21e");

        // Seed Manufacturers
        modelBuilder.Entity<Manufacturer>().HasData(
            new Manufacturer { Id = appleId, Name = "Apple Inc." },
            new Manufacturer { Id = sonyId, Name = "Sony Corporation" },
            new Manufacturer { Id = dellId, Name = "Dell Technologies" }
        );

        // Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = iphoneId, Name = "iPhone 15 Pro", ManufacturerId = appleId },
            new Product { Id = macbookId, Name = "MacBook Air M3", ManufacturerId = appleId },
            new Product { Id = ps5Id, Name = "PlayStation 5", ManufacturerId = sonyId },
            new Product { Id = headphonesId, Name = "Sony WH-1000XM5", ManufacturerId = sonyId },
            new Product { Id = xpsId, Name = "XPS 13", ManufacturerId = dellId },
            new Product { Id = alienwareId, Name = "Alienware Aurora R13", ManufacturerId = dellId }
        );
    }
}