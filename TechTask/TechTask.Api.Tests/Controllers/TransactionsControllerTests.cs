using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using TechTask.Api.Models;

namespace TechTask.Api.Tests.Controllers;

public class TransactionControllerTests : IntegrationTestBase
{
    public TransactionControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    private Transaction GetTransactionForTest() => new Transaction
    {
        Date = DateTime.Now,
        ProductId = 1,
        Quantity = 12,
        TotalAmount = 3.21,
        Type = TransactionType.Sale
    };

    [Fact]
    public async Task PostTransaction_Should_Return_Created()
    {
        var product = new Product
        {
            Name = "Test Product",
            Price = 10.0,
            StockQuantity = 30,
            CategoryId = 1,
            SupplierId = 1,
        };

        var productResponse = await Client.PostAsJsonAsync("/api/products", product);
        productResponse.EnsureSuccessStatusCode();

        var createdProduct = await productResponse.Content.ReadFromJsonAsync<Product>();

        var transaction = GetTransactionForTest();
        transaction.ProductId = createdProduct.Id;

        var response = await Client.PostAsJsonAsync("/api/transactions", transaction);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostTransaction_With_Invalid_model_Should_Return_BadRequest()
    {
        var transaction = GetTransactionForTest();
        transaction.Quantity = 0;

        var response = await Client.PostAsJsonAsync("/api/transactions", transaction);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTransaction_Should_Return_Ok()
    {
        var response = await Client.GetAsync("/api/transactions");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTransactionById_With_invalid_Id_Should_Return_NotFound()
    {
        var invalidId = 9999;

        var response = await Client.GetAsync($"/api/transactions/{invalidId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTransaction_Should_Return_MethodNotAllowed()
    {
        var transaction = GetTransactionForTest();

        var response = await Client.PutAsJsonAsync($"/api/transactions/{transaction.Id}", transaction);

        //Transaction can be successful or not, no updates(No PUT method)!
        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
    }


    [Fact]
    public async Task DeleteTransaction_Should_Return_NoContent()
    {
        var product = new Product
        {
            Name = "Test Product",
            Price = 10.0,
            StockQuantity = 30,
            CategoryId = 1,
            SupplierId = 1,
        };

        var productResponse = await Client.PostAsJsonAsync("/api/products", product);
        productResponse.EnsureSuccessStatusCode();

        var createdProduct = await productResponse.Content.ReadFromJsonAsync<Product>();

        var transaction = new Transaction
        {
            Date = DateTime.Now,
            ProductId = createdProduct.Id,
            Quantity = 12,
            TotalAmount = 3.21,
            Type = TransactionType.Sale
        };

        var post = await Client.PostAsJsonAsync("/api/transactions", transaction);
        Assert.Equal(HttpStatusCode.Created, post.StatusCode);

        var createdTransaction = await post.Content.ReadFromJsonAsync<Transaction>();

        var response = await Client.DeleteAsync($"/api/transactions/{createdTransaction.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTransaction_WithInvalidId_Should_Return_NotFound()
    {
        var transaction = GetTransactionForTest();
        transaction.Id = 9999; // Assuming this doesn't exist

        var response = await Client.DeleteAsync($"/api/transactions/{transaction.Id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}