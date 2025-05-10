using AssignmentAPI.IdentityManagers;
using AssignmentAPI.Models;
using AssignmentAPI.Models.Requests;
using AssignmentAPI.Models.Settings;
using AssignmentAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AssignmentAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class UsersController
        (
        AppDbContext _context,
        UserManager userManager,
        IOptions<AppSettings> appSettings
        ) : APIControllerBase
    {
        [HttpPost("SignUp")]
        [Authorize(nameof(ApplicationRole.Admin))]
        public async Task<IResult> SignUp(SignUpRequest signUpRequest)
        {
            var result = await userManager.AddUserAsync(signUpRequest.GetUserRegistrationRequest());

            return result ? Results.Ok(result) : Results.BadRequest(result);
        }

        [HttpPost("SignIn")]
        [AllowAnonymous]
        public async Task<IResult> SignIn(SignInRequest signInRequest)
        {
            var user = await userManager.FindByEmailAsync(signInRequest.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, signInRequest.Password))
            {
                var roles = await userManager.GetRolesAsync(user);
                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.JWTSecret));
                ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userId",user.Id.ToString()),
                    new Claim(ClaimTypes.Role,roles.First()),
                });

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature),
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Results.Ok(new { token });
            }
            else
                return Results.BadRequest(new { message = "Username or password is incorrect." });
        }

        [HttpGet("GetAll")]
        [Authorize(nameof(ApplicationRole.Admin))]
        public async Task<IResult> Index()
        {
            var result = await _context.Users
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.Email,
                    x.FullName,
                })
                .ToListAsync();

            return Results.Ok(result);
        }

        [HttpGet("GetDetails/{id}")]
        [Authorize(nameof(ApplicationRole.Admin))]
        public async Task<IResult> Details(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.Email,
                    x.FullName,
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            return user == null
                ? Results.NotFound()
                : Results.Ok(user);
        }

        [HttpPut("User")]
        [Authorize(nameof(ApplicationRole.Admin))]
        public async Task<IResult> Edit(UpdateUserRequest userRequest, [FromServices] UpdateUserRequestValidator validator)
        {
            var result = await validator.ValidateAsync(userRequest);

            if (!result.IsValid)
            {
                return Results.BadRequest("Not Valid Values:" + string.Join(',', result.Errors));
            }
            try
            {
                var user = await _context.Users
                    .Where(x => !x.IsDeleted)
                    .FirstOrDefaultAsync(m => m.Id == userRequest.Id);

                user.FullName = userRequest.FullName;
                user.UserName = userRequest.UserName;

                _context.Update(user);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userRequest.Id))
                {
                    return Results.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Results.Ok(userRequest);
        }

        [HttpDelete("User/{id}")]
        [Authorize(nameof(ApplicationRole.Admin))]
        public async Task<IResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                if (user.Id.Equals(CurrentUserId))
                {
                    return Results.BadRequest("You can't Delete Yourself");
                }

                user.IsDeleted = true;
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        private bool UserExists(int id)
        {
            return _context.Users
                .Where(x => !x.IsDeleted)
                .Any(e => e.Id == id);
        }
    }
}
