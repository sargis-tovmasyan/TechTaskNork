using Microsoft.EntityFrameworkCore;
using TechTask.Api.Database;
using TechTask.Api.Interfaces;
using TechTask.Api.Models;
using TechTask.Api.Services;

namespace TechTask.Api.Tests
{
    public abstract class UnitTestBase
    {
        protected AppDbContext DbContext { get; private set; }

        // Services
        protected IProductsService ProductsService { get; private set; }
        protected ICategoriesService CategoriesService { get; private set; }
        protected ISuppliersService SupplierService { get; private set; }
        protected ITransactionsService TransactionsService { get; private set; }

        protected UnitTestBase()
        {
            // Initialize in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique DB per test class
                .Options;

            DbContext = new AppDbContext(options);

            // Seed initial test data
            SeedTestData();

            // Initialize services
            ProductsService = new ProductsService(DbContext);
            CategoriesService = new CategoriesService(DbContext);
            SupplierService = new SuppliersService(DbContext);
            TransactionsService = new TransactionsService(DbContext);
        }

        private void SeedTestData()
        {
            var category1 = new Category { Id = 1, Name = "Electronics" };
            var category2 = new Category { Id = 2, Name = "Books" };

            var supplier1 = new Supplier { Id = 1, Name = "Supplier A" };
            var supplier2 = new Supplier { Id = 2, Name = "Supplier B" };

            var product1 = new Product
            {
                Id = 1,
                Name = "Smartphone",
                Price = 299.99,
                StockQuantity = 50,
                CategoryId = category1.Id,
                SupplierId = supplier1.Id
            };

            var product2 = new Product
            {
                Id = 2,
                Name = "E-Reader",
                Price = 129.99,
                StockQuantity = 30,
                CategoryId = category1.Id,
                SupplierId = supplier2.Id
            };

            var product3 = new Product
            {
                Id = 3,
                Name = "Novel Book",
                Price = 19.99,
                StockQuantity = 100,
                CategoryId = category2.Id,
                SupplierId = supplier1.Id
            };

            var transaction1 = new Transaction
            {
                Id = 1,
                Date = DateTime.UtcNow.AddDays(-5),
                Quantity = 10,
                TotalAmount = 2999.90,
                Type = TransactionType.Purchase,
                ProductId = product1.Id
            };

            var transaction2 = new Transaction
            {
                Id = 2,
                Date = DateTime.UtcNow.AddDays(-2),
                Quantity = 5,
                TotalAmount = 649.95,
                Type = TransactionType.Sale,
                ProductId = product2.Id
            };

            DbContext.Categories.AddRange(category1, category2);
            DbContext.Suppliers.AddRange(supplier1, supplier2);
            DbContext.Products.AddRange(product1, product2, product3);
            DbContext.Transactions.AddRange(transaction1, transaction2);

            DbContext.SaveChanges();
        }
    }
}
