using AssignmentAPI.Models;
using AssignmentAPI.Models.Requests;
using AssignmentAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentAPI.Controllers
{
    [ApiController]
    [Route("api/Invoices")]
    public class InvoicesController : APIControllerBase
    {
        private readonly AppDbContext _context;

        public InvoicesController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IResult> Index()
        {
            var result = (await _context.Invoices
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Details)
                .ThenInclude(d => d.Product)
                .Where(x => !x.IsDeleted)
                .ToListAsync())
                .Select(x => new
                {
                    x.Id,
                    x.Date,
                    User = x.User.FullName,
                    TotalAmount = x.Details.Sum(d => d.Quantity * d.Product.Price),
                    Details = x.Details.Select(d => new
                    {
                        d.Id,
                        d.Product.ArabicName,
                        d.Product.EnglishName,
                        d.Product.Price,
                        d.Quantity,
                    })
                });

            return Results.Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IResult> Details(int id)
        {
            var invoice = (await _context.Invoices
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Details)
                .ThenInclude(d => d.Product)
                .Where(x => !x.IsDeleted && x.Id.Equals(id))
                .ToListAsync())
                .Select(x => new
                {
                    x.Id,
                    x.Date,
                    User = x.User.FullName,
                    TotalAmount = x.Details.Sum(d => d.Quantity * d.Product.Price),
                    Details = x.Details.Select(d => new
                    {
                        d.Id,
                        d.Product.ArabicName,
                        d.Product.EnglishName,
                        d.Product.Price,
                        d.Quantity,
                    })
                })
                .FirstOrDefault();

            return invoice == null
                ? Results.NotFound()
                : Results.Ok(invoice);
        }

        [HttpPost("Generate")]
        public async Task<IResult> Create(CreateInvoiceRequest invoice, [FromServices] CreateInvoiceRequestValidator validator)
        {
            var result = await validator.ValidateAsync(invoice);

            if (!result.IsValid)
            {
                return Results.BadRequest("Not Valid Invoice:" + string.Join(',', result.Errors));
            }

            var invoiceModel = invoice.ToInvoice();

            _context.Add(invoiceModel);

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        [HttpDelete]
        public async Task<IResult> Delete(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);

            if (invoice == null)
            {
                return Results.BadRequest("Not Exists Id");
            }

            invoice.IsDeleted = true;
            _context.Invoices.Update(invoice);

            await _context.SaveChangesAsync();

            return Results.Ok();
        }
    }
}
