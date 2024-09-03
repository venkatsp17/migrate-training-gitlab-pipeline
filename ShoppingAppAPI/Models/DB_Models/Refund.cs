using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Models
{
    public class Refund
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RefundID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime? Refund_Date { get; set; }
        [Required]
        public string Reason { get; set; }
        public string Refund_Method { get; set; }
        public RefundStatus Status { get; set; }
        public Order Order { get; set; }
    }
}
