using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s.Review_DTO_s
{
    public class ReviewGetDTO
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [MaxLength(1000, ErrorMessage = "Comment cannot be longer than 1000 characters.")]
        public string? Comment { get; set; }
    }
}
