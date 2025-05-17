using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using TechTask.Api.Models;

namespace TechTask.Api.Tests.Controllers;

public class CategoriesControllerTests : IntegrationTestBase
{
    public CategoriesControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    private Category GetCategoryForTest() => new Category
    {
        Name = "Test Category"
    };

    [Fact]
    public async Task PostCategory_Should_Return_Created()
    {
        var response = await Client.PostAsJsonAsync("/api/categories", GetCategoryForTest());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostCategory_With_Invalid_model_Should_Return_BadRequest()
    {
        var category = GetCategoryForTest();
        category.Name = string.Empty;

        var response = await Client.PostAsJsonAsync("/api/categories", category);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }



    [Fact]
    public async Task GetCategories_Should_Return_Ok()
    {
        var response = await Client.GetAsync("/api/categories");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCategoryById_With_invalid_Id_Should_Return_NotFound()
    {
        var invalidId = 9999;

        var response = await Client.GetAsync($"/api/categories/{invalidId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    [Fact]
    public async Task UpdateCategory_Should_Return_NoContent()
    {
        var post = await Client.PostAsJsonAsync("/api/categories", GetCategoryForTest());
        var created = await post.Content.ReadFromJsonAsync<Category>();

        created.Name = "Updated";

        var response = await Client.PutAsJsonAsync($"/api/categories/{created.Id}", created);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCategory_With_Invalid_id_Should_Return_NotFound()
    {
        var category = GetCategoryForTest();
        category.Id = 9999; // Assuming this doesn't exist

        var response = await Client.PutAsJsonAsync($"/api/categories/{category.Id}", category);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    [Fact]
    public async Task DeleteCategory_Should_Return_NoContent()
    {
        var post = await Client.PostAsJsonAsync("/api/categories", GetCategoryForTest());
        var created = await post.Content.ReadFromJsonAsync<Category>();

        var response = await Client.DeleteAsync($"/api/categories/{created.Id}");

        Assert.Equal(HttpStatusCode.Created, post.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCategory_WithInvalidId_Should_Return_NotFound()
    {
        var category = GetCategoryForTest();
        category.Id = 9999; // Assuming this doesn't exist

        var response = await Client.DeleteAsync($"/api/categories/{category.Id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}