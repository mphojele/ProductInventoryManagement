namespace ProductInventoryManagement.WebAPIControllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ProductInventoryManagement.Data;
    using ProductInventoryManagement.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductInventoryWebAPIController : ControllerBase
    {
        private readonly ProductInventoryContext context;

        public ProductInventoryWebAPIController(ProductInventoryContext context)
        {
            this.context = context;
        }

        // GET: api/ProductInventoryWebAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await this.context.Product.ToListAsync();
        }

        // GET: api/ProductInventoryWebAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await this.context.Product.FindAsync(id);

            if (product == null)
            {
                return this.NotFound();
            }

            return product;
        }

        // PUT: api/ProductInventoryWebAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return this.BadRequest();
            }

            this.context.Entry(product).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.ProductExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.NoContent();
        }

        // POST: api/ProductInventoryWebAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            this.context.Product.Add(product);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction("GetProduct", new { id = product.ProductID }, product);
        }

        // DELETE: api/ProductInventoryWebAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await this.context.Product.FindAsync(id);
            if (product == null)
            {
                return this.NotFound();
            }

            this.context.Product.Remove(product);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        private bool ProductExists(int id)
        {
            return this.context.Product.Any(e => e.ProductID == id);
        }
    }
}
