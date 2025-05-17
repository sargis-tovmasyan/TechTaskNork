using TechTask.Api.Models;

namespace TechTask.Api.Tests.Services;

public class ProductsServiceTests : UnitTestBase
{
    private Product GetProductForTest() => new Product
    {
        Name = "Test Product",
        Price = 10.0,
        StockQuantity = 100,
        CategoryId = 1,
        SupplierId = 1,
    };

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Products()
    {
        var result = await ProductsService.GetAllAsync();

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Product_If_Exists()
    {
        var result = await ProductsService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Smartphone", result.Name);
    }


    [Fact]
    public async Task PostProductAsync_Should_Add_Product()
    {
        var product = GetProductForTest();

        var result = await ProductsService.PostAsync(product);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateProductAsync_Should_Update_Product()
    {
        var product = GetProductForTest();
        product.Name = "OldName";

        DbContext.Products.Add(product);
        await DbContext.SaveChangesAsync();

        var updatedProduct = new Product
        {
            Id = product.Id,
            Name = "Updated",
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            SupplierId = product.SupplierId
        };

        await ProductsService.UpdateAsync(updatedProduct);

        var fromDb = await DbContext.Products.FindAsync(product.Id);
        Assert.Equal("Updated", fromDb?.Name);
    }

    [Fact]
    public async Task DeleteProductByIdAsync_Should_Delete_Existing_Product()
    {
        var product = GetProductForTest();
        await DbContext.Products.AddAsync(product);
        await DbContext.SaveChangesAsync();

        var result = await ProductsService.DeleteByIdAsync(product.Id);

        Assert.True(result);
    }

}