using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssignmentAPI.Models;
using AssignmentAPI.Models.Requests;
using AssignmentAPI.Validators;
using Microsoft.AspNetCore.Authorization;

namespace AssignmentAPI.Controllers
{
    [ApiController]
    [Route("api/Products")]
    public class ProductsController : APIControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IResult> Index(string sortBy = "ArabicName", bool sortAcsending = true)
        {
            var result = await _context.Products
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            switch (sortBy)
            {
                case "ArabicName":
                    result = !sortAcsending
                        ? result.OrderByDescending(x => x.ArabicName).ToList()
                        : result.OrderBy(x => x.ArabicName).ToList();
                    break;
                case "EnglishName":
                    result = !sortAcsending
                        ? result.OrderByDescending(x => x.EnglishName).ToList()
                        : result.OrderBy(x => x.EnglishName).ToList();
                    break;
            }

            return Results.Ok(result.Select(x => new
            {
                x.Id,
                x.ArabicName,
                x.EnglishName,
                x.Price,
            }));
        }

        [HttpGet("{id}")]
        public async Task<IResult> Details(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(m => m.Id == id);

            return product == null
                ? Results.NotFound()
                : Results.Ok(new
                {
                    product.Id,
                    product.ArabicName,
                    product.EnglishName,
                    product.Price,
                });
        }

        [Authorize(nameof(ApplicationRole.Admin))]
        [HttpPost]
        public async Task<IResult> Create(CreateProductRequest product, [FromServices] CreateProductRequestValidator validator)
        {
            var result = await validator.ValidateAsync(product);

            if (!result.IsValid)
            {
                return Results.BadRequest("Not Valid Product:" + string.Join(',', result.Errors));
            }

            var productModel = product.ToProduct();

            _context.Add(productModel);

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        [Authorize(nameof(ApplicationRole.Admin))]
        [HttpPut]
        public async Task<IResult> Edit([FromBody] UpdateProductRequest product, [FromServices] UpdateProductRequestValidator validator)
        {
            var result = await validator.ValidateAsync(product);

            if (!result.IsValid)
            {
                return Results.BadRequest("Not Valid Product:" + string.Join(',', result.Errors));
            }
            try
            {
                var original = _context.Products.FirstOrDefault(x => x.Id.Equals(product.Id));

                original.EnglishName = product.EnglishName;
                original.ArabicName = product.ArabicName;
                original.Price = product.Price;

                _context.Update(original);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return Results.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Results.Ok(product);
        }

        [Authorize(nameof(ApplicationRole.Admin))]
        [HttpDelete]
        public async Task<IResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return Results.BadRequest("Not Exists Id");
            }

            product.IsDeleted = true;
            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        private bool ProductExists(int id)
        {
            return _context.Products
                .Where(x => !x.IsDeleted)
                .Any(e => e.Id == id);
        }
    }
}
