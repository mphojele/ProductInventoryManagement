namespace ProductInventoryManagement.Validators
{
    using FluentValidation;

    using ProductInventoryManagement.Models;

    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            this.RuleSet("Post", () =>
            {
                this.GeneralRules();
            });

            this.RuleSet("Put", () =>
            {
                this.GeneralRules();

                this.RuleFor(_ => _.ProductID)
                    .GreaterThan(0);
            });

            this.RuleSet("Delete", () =>
            {
                this.RuleFor(_ => _.ProductID)
                    .GreaterThan(0);
            });
        }

        private void GeneralRules()
        {
            this.RuleFor(_ => _.Name)
                .NotEmpty()
                .MaximumLength(30);

            this.RuleFor(_ => _.Description)
                .NotEmpty()
                .MaximumLength(50);

            this.RuleFor(_ => _.Price)
                .InclusiveBetween(1.00, 100000.00);

            this.RuleFor(_ => _.Quantity)
                .InclusiveBetween(1, 100);
        }
    }
}