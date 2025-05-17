using TechTask.Api.Models;

namespace TechTask.Api.Tests.Services;

public class TransactionsServiceTests : UnitTestBase
{
    private Transaction GetTransactionForTest() => new Transaction
    {
        Date = DateTime.Now,
        ProductId = 1,
        Quantity = 12,
        TotalAmount = 3.21,
        Type = TransactionType.Sale
    };

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Transactions()
    {
        var result = await TransactionsService.GetAllAsync();

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Transaction_If_Exists()
    {
        var result = await TransactionsService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(10, result.Quantity);
    }


    [Fact]
    public async Task PostTransactionAsync_Should_Add_Transaction()
    {
        var transaction = GetTransactionForTest();

        var result = await TransactionsService.PostAsync(transaction);

        Assert.True(result);
    }

    [Fact]
    public async Task PostTransactionAsync_Sales_Transactions_Decreasing_Products_StockQuantity()
    {
        var transaction = GetTransactionForTest();
        transaction.Id = 123;
        var productBeforeTransaction = await ProductsService.GetByIdAsync(transaction.ProductId);

        var result = await TransactionsService.PostAsync(transaction);
        var postedTransaction = await TransactionsService.GetByIdAsync(transaction.Id);

        Assert.True(result);
        Assert.True(productBeforeTransaction.StockQuantity > postedTransaction.Product.StockQuantity);
    }

    [Fact]
    public async Task PostTransactionAsync_Sales_Transactions_Increasing_Products_StockQuantity()
    {
        var transaction = GetTransactionForTest();
        transaction.Id = 321;
        transaction.Type = TransactionType.Purchase;

        var productBeforeTransaction = await ProductsService.GetByIdAsync(transaction.ProductId);

        var result = await TransactionsService.PostAsync(transaction);
        var postedTransaction = await TransactionsService.GetByIdAsync(transaction.Id);

        Assert.True(result);
        Assert.True(productBeforeTransaction.StockQuantity < postedTransaction.Product.StockQuantity);
    }

    [Fact]
    public async Task UpdateTransactionAsync_Should_Update_Transaction()
    {
        var transaction = GetTransactionForTest(); //Sale

        DbContext.Transactions.Add(transaction);
        await DbContext.SaveChangesAsync();

        var updatedTransaction = new Transaction //Changing to purchase
        {
            Id = transaction.Id,
            Quantity = 222,
            ProductId = transaction.ProductId,
            Date = transaction.Date,
            TotalAmount = transaction.TotalAmount,
            Type = TransactionType.Purchase
        };

        await TransactionsService.UpdateAsync(updatedTransaction);

        var fromDb = await DbContext.Transactions.FindAsync(transaction.Id);
        Assert.True(fromDb?.Type == TransactionType.Purchase);
    }

    [Fact]
    public async Task DeleteTransactionByIdAsync_Should_Delete_Existing_Transaction()
    {
        var transaction = GetTransactionForTest();
        await DbContext.Transactions.AddAsync(transaction);
        await DbContext.SaveChangesAsync();

        var result = await TransactionsService.DeleteByIdAsync(transaction.Id);

        Assert.True(result);
    }

}