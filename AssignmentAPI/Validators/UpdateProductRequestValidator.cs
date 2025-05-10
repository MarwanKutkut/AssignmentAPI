using AssignmentAPI.Models;
using AssignmentAPI.Models.Requests;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AssignmentAPI.Validators
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        private readonly AppDbContext _context;

        public UpdateProductRequestValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).MustAsync(ProductIdExists);
            RuleFor(x => x.EnglishName).NotEmpty();
            RuleFor(x => x.ArabicName).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
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
