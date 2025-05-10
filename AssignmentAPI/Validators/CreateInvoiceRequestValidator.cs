using AssignmentAPI.Models;
using AssignmentAPI.Models.Requests;
using FluentValidation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace AssignmentAPI.Validators
{
    public class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
    {
        private readonly AppDbContext _context;

        public CreateInvoiceRequestValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.UserId).NotEmpty().MustAsync(UserIdExists);
            RuleFor(x => x.Details).Must(x => x.Any());
            RuleForEach(x => x.Details).SetValidator(new CreateInvoiceDetailRequestValidator(_context));
        }

        private async Task<bool> UserIdExists(int id, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted && u.Id.Equals(id))
                .FirstOrDefaultAsync(cancellationToken) != null;
        }
    }

    public class CreateInvoiceDetailRequestValidator : AbstractValidator<CreateInvoiceDetailRequest>
    {
        private readonly AppDbContext _context;

        public CreateInvoiceDetailRequestValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.ProductId).NotEmpty().MustAsync(ProductIdExists);
            RuleFor(x => x.Quantity).GreaterThan(0);
        }

        private async Task<bool> ProductIdExists(int id, CancellationToken cancellationToken)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(u => !u.IsDeleted && u.Id.Equals(id))
                .FirstOrDefaultAsync(cancellationToken) != null;
        }
    }
}
