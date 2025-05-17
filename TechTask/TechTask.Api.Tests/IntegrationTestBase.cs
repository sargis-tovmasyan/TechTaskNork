using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechTask.Api.Database;
using TechTask.Api.Models;

namespace TechTask.Api.Tests;

public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;
    private readonly string _dbFilePath;
    private readonly DbConnection _connection;
    private bool _disposed;

    protected IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        _dbFilePath = Path.Combine(Path.GetTempPath(), $"TestDb_{Guid.NewGuid():N}.sqlite");

        _connection = new SqliteConnection($"Data Source={_dbFilePath}");
        _connection.Open();

        var appFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (dbContextDescriptor != null)
                    services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbConnection));
                if (dbConnectionDescriptor != null)
                    services.Remove(dbConnectionDescriptor);

                services.AddSingleton(_connection);

                services.AddDbContext<AppDbContext>((container, options) =>
                {
                    options.UseSqlite(_connection);
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();

                // Seeding Category and Supplier
                context.Categories.Add(new Category { Id = 1, Name = "Test Category" });
                context.Suppliers.Add(new Supplier { Id = 1, Name = "Test Supplier" });

                context.SaveChanges();
            });

            builder.UseEnvironment("Development");
        });

        Client = appFactory.CreateClient();
    }
}