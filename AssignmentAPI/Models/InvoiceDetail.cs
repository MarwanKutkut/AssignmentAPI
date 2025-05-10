using System.ComponentModel.DataAnnotations.Schema;

namespace AssignmentAPI.Models
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))] public virtual Product Product { get; set; }
        public decimal Quantity { get; set; }
    }
}
