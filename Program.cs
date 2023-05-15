namespace ProductInventoryManagement
{
    using FluentValidation;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using ProductInventoryManagement.Data;
    using ProductInventoryManagement.Models;
    using ProductInventoryManagement.Validators;

    public static class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ProductInventoryContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ProductInventoryContext") ?? throw new InvalidOperationException("Connection string 'ProductInventoryContext' not found.")));

            // Add services to the container.
            builder.Services
                .AddControllersWithViews()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            builder.Services.AddScoped<IValidator<Product>, ProductValidator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
