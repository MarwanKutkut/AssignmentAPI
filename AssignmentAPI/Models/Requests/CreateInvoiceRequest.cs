using System.ComponentModel.DataAnnotations.Schema;

namespace AssignmentAPI.Models.Requests
{
    public class CreateInvoiceRequest
    {
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public List<CreateInvoiceDetailRequest> Details { get; set; }
        public Invoice ToInvoice()
        {
            return new Invoice
            {
                Date = Date,
                UserId = UserId,
                Details = Details.Select(x => new InvoiceDetail
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                })
                .ToList(),
            };
        }
    }
    
    public class CreateInvoiceDetailRequest
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
    }
}
