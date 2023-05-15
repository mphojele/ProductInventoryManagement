namespace ProductInventoryManagement.Data
{
    using Microsoft.EntityFrameworkCore;
    using ProductInventoryManagement.Models;

    public class ProductInventoryContext : DbContext
    {
        public ProductInventoryContext(DbContextOptions<ProductInventoryContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; } = default!;
    }
}
