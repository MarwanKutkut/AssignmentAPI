using AssignmentAPI.Models;
using AssignmentAPI.Models.Requests;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AssignmentAPI.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        private readonly AppDbContext _context;

        public UpdateUserRequestValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.UserName).NotNull().NotEmpty();
            RuleFor(x => x.FullName).NotNull().NotEmpty();
            RuleFor(x => x.Id).NotNull().NotEmpty().MustAsync(UserIdExists);
        }

        private async Task<bool> UserIdExists(int id, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted && u.Id.Equals(id))
                .FirstOrDefaultAsync(cancellationToken) != null;
        }
    }
}