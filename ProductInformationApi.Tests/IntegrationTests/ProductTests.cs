using System.Net;
using System.Net.Http.Json;
using ProductInformationApi.Models.DTO;
using ProductInformationApi.Models.Entities;
using ProductInformationApi.Tests.Helpers;

namespace ProductInformationApi.Tests.IntegrationTests;

public class ProductTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetProducts_ReturnsListOfProducts()
    {
        // Arrange: Create a manufacturer and two products
        var manufacturerResponse = await _client.PostAsJsonAsync("/manufacturers", new
        {
            name = "Test Manufacturer"
        });

        Assert.Equal(HttpStatusCode.Created, manufacturerResponse.StatusCode);

        var manufacturer = await manufacturerResponse.Content.ReadFromJsonAsync<Manufacturer>();
        Assert.NotNull(manufacturer);

        var testProduct1 = new { name = "Test Product A", manufacturerId = manufacturer!.Id };
        var testProduct2 = new { name = "Test Product B", manufacturerId = manufacturer.Id };

        var postProduct1 = await _client.PostAsJsonAsync("/products", testProduct1);
        var postProduct2 = await _client.PostAsJsonAsync("/products", testProduct2);

        Assert.Equal(HttpStatusCode.Created, postProduct1.StatusCode);
        Assert.Equal(HttpStatusCode.Created, postProduct2.StatusCode);

        // Act: Call GET /products
        var response = await _client.GetAsync("/products");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content.ReadFromJsonAsync<List<GetProductResponseDto>>();
        Assert.NotNull(products);
        Assert.True(products!.Count >= 2);

        Assert.Contains(products, p => p.Name == "Test Product A");
        Assert.Contains(products, p => p.Name == "Test Product B");
    }

    [Fact]
    public async Task GetProductById_ReturnsProduct_WhenExists()
    {
        // Arrange: create manufacturer and product
        var manufacturerResponse = await _client.PostAsJsonAsync("/manufacturers", new
        {
            name = "Test Manufacturer"
        });
        var manufacturer = await manufacturerResponse.Content.ReadFromJsonAsync<Manufacturer>();

        var productRequest = new { name = "Single Product", manufacturerId = manufacturer!.Id };
        var postResponse = await _client.PostAsJsonAsync("/products", productRequest);
        var createdProduct = await postResponse.Content.ReadFromJsonAsync<Product>();

        // Act
        var getResponse = await _client.GetAsync($"/products/{createdProduct!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var product = await getResponse.Content.ReadFromJsonAsync<GetProductResponseDto>();
        Assert.NotNull(product);
        Assert.Equal("Single Product", product!.Name);
        Assert.Equal(manufacturer.Id, product.ManufacturerId);
    }

    [Fact]
    public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync($"/products/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostProduct_CreatesProduct_WhenManufacturerExists()
    {
        // Arrange: Create manufacturer
        var manufacturerResponse = await _client.PostAsJsonAsync("/manufacturers", new
        {
            name = "Maker Inc"
        });
        var manufacturer = await manufacturerResponse.Content.ReadFromJsonAsync<Manufacturer>();

        // Act
        var productRequest = new { name = "New Gadget", manufacturerId = manufacturer!.Id };
        var response = await _client.PostAsJsonAsync("/products", productRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<GetProductResponseDto>();
        Assert.NotNull(created);
        Assert.Equal("New Gadget", created!.Name);
        Assert.Equal(manufacturer.Id, created.ManufacturerId);
    }

    [Fact]
    public async Task PostProduct_ReturnsServerError_WhenManufacturerDoesNotExist()
    {
        // Arrange: manufacturerId is random
        var request = new
        {
            name = "Invalid Manufacturer Product",
            manufacturerId = Guid.NewGuid()
        };

        // Act
        var response = await _client.PostAsJsonAsync("/products", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}