using AssignmentAPI.Models;
using FluentValidation;

namespace AssignmentAPI.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ArabicName).NotNull().NotEmpty();
            RuleFor(x => x.EnglishName).NotNull().NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
        }
    }
}