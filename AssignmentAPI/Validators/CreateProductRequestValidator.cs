using AssignmentAPI.Models.Requests;
using FluentValidation;

namespace AssignmentAPI.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.EnglishName).NotEmpty();
            RuleFor(x => x.ArabicName).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}
