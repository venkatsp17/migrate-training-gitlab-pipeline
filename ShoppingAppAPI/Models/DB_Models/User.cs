using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public byte[] Password { get; set; }
        [Required]
        public byte[] Password_Hashkey { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        [Required]
        public UserRole Role { get; set; }
        public Customer Customer { get; set; }
        public Seller Seller { get; set; }
    }
}
