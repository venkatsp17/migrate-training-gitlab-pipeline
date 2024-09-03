using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        [Required]
        public DateTime Review_Date { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
    }
}
