using System.Net;
using System.Net.Http.Json;
using ProductInformationApi.Models;
using ProductInformationApi.Tests.Helpers;

namespace ProductInformationApi.Tests.IntegrationTests;

public class ManufacturerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetManufacturers_ReturnsListOfManufacturers()
    {
        // Arrange
        var response1 = await _client.PostAsJsonAsync("/manufacturers", new { name = "Maker One" });
        var response2 = await _client.PostAsJsonAsync("/manufacturers", new { name = "Maker Two" });

        Assert.Equal(HttpStatusCode.Created, response1.StatusCode);
        Assert.Equal(HttpStatusCode.Created, response2.StatusCode);

        // Act
        var getResponse = await _client.GetAsync("/manufacturers");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var manufacturers = await getResponse.Content.ReadFromJsonAsync<List<Manufacturer>>();
        Assert.NotNull(manufacturers);
        Assert.True(manufacturers!.Count >= 2);
        Assert.Contains(manufacturers, m => m.Name == "Maker One");
        Assert.Contains(manufacturers, m => m.Name == "Maker Two");
    }

    [Fact]
    public async Task GetManufacturerById_ReturnsManufacturer_WhenExists()
    {
        // Arrange
        var postResponse = await _client.PostAsJsonAsync("/manufacturers", new { name = "Test Co" });
        var created = await postResponse.Content.ReadFromJsonAsync<Manufacturer>();

        // Act
        var getResponse = await _client.GetAsync($"/manufacturers/{created!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var manufacturer = await getResponse.Content.ReadFromJsonAsync<Manufacturer>();
        Assert.NotNull(manufacturer);
        Assert.Equal("Test Co", manufacturer!.Name);
    }

    [Fact]
    public async Task GetManufacturerById_ReturnsNotFound_WhenNotExists()
    {
        var response = await _client.GetAsync($"/manufacturers/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetProductsByManufacturerId_ReturnsProducts_WhenTheyExist()
    {
        // Arrange: create manufacturer
        var manufacturerResponse = await _client.PostAsJsonAsync("/manufacturers", new { name = "Device Corp" });
        var manufacturer = await manufacturerResponse.Content.ReadFromJsonAsync<Manufacturer>();

        // Post products
        var product1 = new { name = "Gizmo", manufacturerId = manufacturer!.Id };
        var product2 = new { name = "Widget", manufacturerId = manufacturer.Id };

        await _client.PostAsJsonAsync("/products", product1);
        await _client.PostAsJsonAsync("/products", product2);

        // Act
        var response = await _client.GetAsync($"/manufacturers/{manufacturer.Id}/products");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.NotNull(products);
        Assert.Equal(2, products!.Count);
        Assert.Contains(products, p => p.Name == "Gizmo");
        Assert.Contains(products, p => p.Name == "Widget");
    }

    [Fact]
    public async Task PostManufacturer_CreatesManufacturer_WhenValid()
    {
        var request = new { name = "New Manufacturer" };

        var response = await _client.PostAsJsonAsync("/manufacturers", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<Manufacturer>();
        Assert.NotNull(created);
        Assert.Equal("New Manufacturer", created!.Name);
        Assert.NotEqual(Guid.Empty, created.Id);
    }
}