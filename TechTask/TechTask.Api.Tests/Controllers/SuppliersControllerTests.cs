using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using TechTask.Api.Models;

namespace TechTask.Api.Tests.Controllers;

public class SuppliersControllerTests : IntegrationTestBase
{
    public SuppliersControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    private Supplier GetSupplierForTest() => new Supplier
    {
        Name = "Test Supplier"
    };

    [Fact]
    public async Task PostSupplier_Should_Return_Created()
    {
        var response = await Client.PostAsJsonAsync("/api/suppliers", GetSupplierForTest());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostSupplier_With_Invalid_model_Should_Return_BadRequest()
    {
        var supplier = GetSupplierForTest();
        supplier.Name = string.Empty;

        var response = await Client.PostAsJsonAsync("/api/suppliers", supplier);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetSuppliers_Should_Return_Ok()
    {
        var response = await Client.GetAsync("/api/suppliers");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetSupplierById_With_invalid_Id_Should_Return_NotFound()
    {
        var invalidId = 9999;

        var response = await Client.GetAsync($"/api/suppliers/{invalidId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    [Fact]
    public async Task UpdateSupplier_Should_Return_NoContent()
    {
        var post = await Client.PostAsJsonAsync("/api/suppliers", GetSupplierForTest());
        var created = await post.Content.ReadFromJsonAsync<Supplier>();

        created.Name = "Updated";

        var response = await Client.PutAsJsonAsync($"/api/suppliers/{created.Id}", created);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateSupplier_With_Invalid_id_Should_Return_NotFound()
    {
        var supplier = GetSupplierForTest();
        supplier.Id = 9999; // Assuming this doesn't exist

        var response = await Client.PutAsJsonAsync($"/api/suppliers/{supplier.Id}", supplier);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    [Fact]
    public async Task DeleteSupplier_Should_Return_NoContent()
    {
        var post = await Client.PostAsJsonAsync("/api/suppliers", GetSupplierForTest());
        var created = await post.Content.ReadFromJsonAsync<Supplier>();

        var response = await Client.DeleteAsync($"/api/suppliers/{created.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteSupplier_WithInvalidId_Should_Return_NotFound()
    {
        var supplier = GetSupplierForTest();
        supplier.Id = 9999; // Assuming this doesn't exist

        var response = await Client.DeleteAsync($"/api/suppliers/{supplier.Id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}