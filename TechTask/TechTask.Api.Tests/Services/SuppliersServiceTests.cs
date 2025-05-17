using TechTask.Api.Models;

namespace TechTask.Api.Tests.Services;

public class SuppliersServiceTests : UnitTestBase
{
    private Supplier GetSupplierForTest() => new Supplier
    {
        Name = "Test Supplier"
    };

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Suppliers()
    {
        var result = await SupplierService.GetAllAsync();

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Supplier_If_Exists()
    {
        var result = await SupplierService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Supplier A", result.Name);
    }


    [Fact]
    public async Task PostSupplierAsync_Should_Add_Supplier()
    {
        var supplier = GetSupplierForTest();

        var result = await SupplierService.PostAsync(supplier);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateSupplierAsync_Should_Update_Supplier()
    {
        var supplier = GetSupplierForTest();
        supplier.Name = "OldName";

        DbContext.Suppliers.Add(supplier);
        await DbContext.SaveChangesAsync();

        var updatedSupplier = new Supplier
        {
            Id = supplier.Id,
            Name = "Updated"
        };

        await SupplierService.UpdateAsync(updatedSupplier);

        var fromDb = await DbContext.Suppliers.FindAsync(supplier.Id);
        Assert.Equal("Updated", fromDb?.Name);
    }

    [Fact]
    public async Task DeleteSupplierByIdAsync_Should_Delete_Existing_Supplier()
    {
        var supplier = GetSupplierForTest();
        await DbContext.Suppliers.AddAsync(supplier);
        await DbContext.SaveChangesAsync();

        var result = await SupplierService.DeleteByIdAsync(supplier.Id);

        Assert.True(result);
    }

}