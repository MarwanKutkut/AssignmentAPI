using AssignmentAPI.Models;
using FluentValidation;

namespace AssignmentAPI.Validators
{
    public class InvoiceDetailValidator : AbstractValidator<InvoiceDetail>
    {
        public InvoiceDetailValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
        }
    }
}
