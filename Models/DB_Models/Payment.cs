using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public string Payment_Method { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Transaction_Date { get; set; }
        [Required]
        public PaymentStatus Payment_Status { get; set; }
        public Order Order { get; set; }
    }
}