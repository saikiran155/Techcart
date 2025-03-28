using System.ComponentModel.DataAnnotations;

namespace Techcart.Models
{
    public class RegisterViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50)]

        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]

        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password should Match")]
        public string ConfirmPassword { get; set; }

        public string? Role { get; set; }  // Default

    }
   
}

