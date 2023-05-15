namespace ProductInventoryManagement.WebAPIControllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using DevExtreme.AspNet.Data;
    using DevExtreme.AspNet.Data.ResponseModel;
    using DevExtreme.AspNet.Mvc;

    using FluentValidation;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;

    using ProductInventoryManagement.Data;
    using ProductInventoryManagement.Models;
    using ProductInventoryManagement.Validators;

    [Route("api/[controller]")]
    public class ProductInventoryWebAPIController : ControllerBase
    {
        private readonly ProductInventoryContext context;
        private readonly IValidator<Product> validator;

        public ProductInventoryWebAPIController(
            ProductInventoryContext context,
            IValidator<Product> validator)
        {
            this.context = context;
            this.validator = validator;
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

            var validationResult = this.validator.Validate(model, _ => _.IncludeRuleSets("Put"));
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);

                return this.BadRequest(this.ModelState);
            }

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

            var validationResult = this.validator.Validate(model, _ => _.IncludeRuleSets("Post"));
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);

                return this.BadRequest(this.ModelState);
            }

            this.context.Product.Add(model);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction("GetProduct", new { id = model.ProductID }, model);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int key)
        {
            var model = new Product()
            {
                ProductID = key,
            };

            var validationResult = this.validator.Validate(model, _ => _.IncludeRuleSets("Delete"));
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);

                return this.BadRequest(this.ModelState);
            }

            if (!this.ProductExists(key))
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
