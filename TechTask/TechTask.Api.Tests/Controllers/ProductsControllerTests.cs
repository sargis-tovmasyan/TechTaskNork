using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using TechTask.Api.Models;

namespace TechTask.Api.Tests.Controllers;

public class ProductsControllerTests
{
    public class ProductsApiTests : IntegrationTestBase
    {
        public ProductsApiTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task PostProduct_Should_Return_Created()
        {
            var product = new { Name = "Test", Price = 10, Stock = 100 };

            var response = await Client.PostAsJsonAsync("/api/products", product);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
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
            var create = new { Name = "ToUpdate", Price = 20 };
            var post = await Client.PostAsJsonAsync("/api/products", create);
            var created = await post.Content.ReadFromJsonAsync<Product>();

            created.Name = "Updated";

            var response = await Client.PutAsJsonAsync($"/api/products/{created.Id}", created);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_Should_Return_NoContent()
        {
            var create = new { Name = "ToDelete", Price = 5 };
            var post = await Client.PostAsJsonAsync("/api/products", create);
            var created = await post.Content.ReadFromJsonAsync<Product>();

            var response = await Client.DeleteAsync($"/api/products/{created.Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

    }

}