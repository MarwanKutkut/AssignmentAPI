using AssignmentAPI.Models.Requests;
using FluentValidation;

namespace AssignmentAPI.Validators
{
    public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
    {
        public UserRegistrationRequestValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(x => x.UserName).NotNull().NotEmpty();
            RuleFor(x => x.FullName).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}