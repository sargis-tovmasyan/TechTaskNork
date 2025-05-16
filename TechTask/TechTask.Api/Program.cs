
using Microsoft.EntityFrameworkCore;
using TechTask.Api.Database;
using TechTask.Api.Interfaces;
using TechTask.Api.Services;

namespace TechTask.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection"));
            });

            builder.Services.AddScoped<IProductService, ProductService>();
            //builder.Services.AddScoped<ICategoryService, CategoryService>();
            //builder.Services.AddScoped<ISupplierService, SupplierService>();
            //builder.Services.AddScoped<ITransactionService, TransactionService>();

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            //DI Mapping
            //builder.Services.AddScoped()

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
