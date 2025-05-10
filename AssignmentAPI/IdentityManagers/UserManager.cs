using AssignmentAPI.Models;
using AssignmentAPI.Models.Requests;
using AssignmentAPI.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AssignmentAPI.IdentityManagers
{
    public class UserManager : UserManager<ApplicationUser>
    {
        private readonly UserRegistrationRequestValidator _validator;

        public UserManager(UserRegistrationRequestValidator validator, IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }
        

        public async Task AddUsersAsync(UserRegistrationRequest[] users)
        {
            if (users == null) throw new ArgumentNullException(nameof(users));

            foreach (var user in users)
            {
                await AddUserAsync(user);
            }
        }

        public async Task<bool> AddUserAsync(UserRegistrationRequest user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var result =  await _validator.ValidateAsync(user);

            if (!result.IsValid)
            {
                throw new Exception("Failed to create user: " + string.Join(", ", result.Errors));
            }

            var userExist = await FindByEmailAsync(user.Email);

            if (userExist != null) return true;

            var adminUser = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                EmailConfirmed = true,
            };

            var CreateUserResult = await CreateAsync(adminUser, user.Password);

            if (!CreateUserResult.Succeeded)
            {
                throw new Exception("Failed to create user: " + string.Join(", ", CreateUserResult.Errors));
            }

            var AddUserToRoleResult = await AddToRoleAsync(adminUser, user.Role);

            if (!AddUserToRoleResult.Succeeded)
            {
                throw new Exception("Failed to Add user to Role: " + string.Join(", ", AddUserToRoleResult.Errors));
            }

            return true;
        }
    }
}
