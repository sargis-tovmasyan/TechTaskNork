using Microsoft.EntityFrameworkCore;
using TechTask.Api.Database;
using TechTask.Api.Models;
using TechTask.Api.Services;

namespace TechTask.Api.Tests.Controllers;

public class ProductsServiceTests
{
    private ProductService GetServiceWithInMemoryDb(out AppDbContext context)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // ensures clean DB for each test
            .Options;

        context = new AppDbContext(options);
        return new ProductService(context);
    }

    [Fact]
    public async Task PostProductAsync_Should_Add_Product()
    {
        var service = GetServiceWithInMemoryDb(out var context);

        var product = new Product { Name = "Test", Price = 10 };

        var result = await service.PostProductAsync(product);

        Assert.True(result);
        Assert.Single(context.Products);
    }

    [Fact]
    public async Task UpdateProductAsync_Should_Update_Product()
    {
        var service = GetServiceWithInMemoryDb(out var context);

        var product = new Product { Name = "Old", Price = 5 };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        product.Name = "Updated";

        var result = await service.UpdateProductAsync(product);

        Assert.True(result);
        Assert.Equal("Updated", context.Products.First().Name);
    }

    [Fact]
    public async Task DeleteProductByIdAsync_Should_Delete_Existing_Product()
    {
        var service = GetServiceWithInMemoryDb(out var context);

        var product = new Product { Name = "ToDelete" };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var result = await service.DeleteProductByIdAsync(product.Id);

        Assert.True(result);
        Assert.Empty(context.Products);
    }

}