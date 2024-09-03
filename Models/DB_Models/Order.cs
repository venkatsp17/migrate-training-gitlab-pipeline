using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        public int? Success_PaymentID { get; set; }
        [Required]
        public DateTime Order_Date { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public decimal Total_Amount { get; set; }
        public string Shipping_Method { get; set; }
        public decimal Shipping_Cost { get; set; }
        public DateTime Last_Updated { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public Refund Refund { get; set; }
    }
}
