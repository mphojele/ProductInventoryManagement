namespace ProductInventoryManagement.WebAPIControllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using DevExtreme.AspNet.Data;
    using DevExtreme.AspNet.Data.ResponseModel;
    using DevExtreme.AspNet.Mvc;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;

    using ProductInventoryManagement.Data;
    using ProductInventoryManagement.Models;

    [Route("api/[controller]")]
    public class ProductInventoryWebAPIController : ControllerBase
    {
        private readonly ProductInventoryContext context;

        public ProductInventoryWebAPIController(ProductInventoryContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<LoadResult> GetProduct(DataSourceLoadOptions loadOptions)
        {
            var models = await this.context.Product.ToListAsync();

            return DataSourceLoader.Load(models, loadOptions);
        }

        [HttpPut]
        public async Task<IActionResult> PutProduct(int key, string values)
        {
            var model = await this.context.Product.FindAsync(key);

            JsonConvert.PopulateObject(values, model);

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.ProductExists(key))
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

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(string values)
        {
            var model = JsonConvert.DeserializeObject<Product>(values);
            this.context.Product.Add(model);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction("GetProduct", new { id = model.ProductID }, model);
        }

        // DELETE: api/ProductInventoryWebAPI
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int key)
        {
            var model = await this.context.Product.FindAsync(key);
            if (model == null)
            {
                return this.NotFound();
            }

            this.context.Product.Remove(model);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        private bool ProductExists(int id)
        {
            return this.context.Product.Any(e => e.ProductID == id);
        }
    }
}
