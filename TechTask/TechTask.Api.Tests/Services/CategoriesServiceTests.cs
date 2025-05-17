using TechTask.Api.Models;

namespace TechTask.Api.Tests.Services;

public class CategoriesServiceTests : UnitTestBase
{
    private Category GetCategoryForTest() => new Category
    {
        Name = "Test Category"
    };

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Categories()
    {
        var result = await CategoriesService.GetAllAsync();

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Category_If_Exists()
    {
        var result = await CategoriesService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Electronics", result.Name);
    }

    [Fact]
    public async Task PostCategoryAsync_Should_Add_Category()
    {
        var category = new Category { Name = "Test" };

        var result = await CategoriesService.PostAsync(category);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateProductAsync_Should_Update_Product()
    {
        var category = GetCategoryForTest();
        category.Name = "OldName";

        DbContext.Categories.Add(category);
        await DbContext.SaveChangesAsync();

        var updatedCategory = new Category()
        {
            Id = category.Id,
            Name = "Updated",
        };

        await CategoriesService.UpdateAsync(updatedCategory);

        var fromDb = await DbContext.Categories.FindAsync(category.Id);
        Assert.Equal("Updated", fromDb?.Name);
    }

    [Fact]
    public async Task DeleteCategoryByIdAsync_Should_Delete_Existing_Category()
    {
        var category = GetCategoryForTest();
        DbContext.Categories.Add(category);
        await DbContext.SaveChangesAsync();

        var result = await CategoriesService.DeleteByIdAsync(category.Id);

        Assert.True(result);
    }

}