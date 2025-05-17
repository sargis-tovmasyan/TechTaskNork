using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using TechTask.Api.Models;

namespace TechTask.Api.Tests.Controllers;

public class ProductsControllerTests : IntegrationTestBase
{
    public ProductsControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    private Product GetProductForTest() => new Product
    {
        Name = "Test Product",
        Price = 10.0,
        StockQuantity = 100,
        CategoryId = 1,
        SupplierId = 1,
    };

    [Fact]
    public async Task PostProduct_Should_Return_Created()
    {
        var response = await Client.PostAsJsonAsync("/api/products", GetProductForTest());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostProduct_With_Invalid_model_Should_Return_BadRequest()
    {
        var product = GetProductForTest();
        product.Name = string.Empty;

        var response = await Client.PostAsJsonAsync("/api/products", product);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }



    [Fact]
    public async Task GetProducts_Should_Return_Ok()
    {
        var response = await Client.GetAsync("/api/products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_Should_Return_NoContent()
    {
        var post = await Client.PostAsJsonAsync("/api/products", GetProductForTest());
        var created = await post.Content.ReadFromJsonAsync<Product>();

        created.Name = "Updated";

        var response = await Client.PutAsJsonAsync($"/api/products/{created.Id}", created);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_With_Invalid_id_Should_Return_NotFound()
    {
        var product = GetProductForTest();
        product.Id = 9999; // Assuming this doesn't exist

        var response = await Client.PutAsJsonAsync($"/api/products/{product.Id}", product);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    [Fact]
    public async Task DeleteProduct_Should_Return_NoContent()
    {
        var post = await Client.PostAsJsonAsync("/api/products", GetProductForTest());
        var created = await post.Content.ReadFromJsonAsync<Product>();

        var response = await Client.DeleteAsync($"/api/products/{created.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_WithInvalidId_Should_Return_NotFound()
    {
        var product = GetProductForTest();
        product.Id = 9999; // Assuming this doesn't exist

        var response = await Client.DeleteAsync($"/api/products/{product.Id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}