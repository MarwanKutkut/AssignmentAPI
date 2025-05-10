using System.ComponentModel.DataAnnotations.Schema;

namespace AssignmentAPI.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))] public virtual ApplicationUser User { get; set; }
        public List<InvoiceDetail> Details { get; set; }
        public bool IsDeleted { get; set; }
    }
}
