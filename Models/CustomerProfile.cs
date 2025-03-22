using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models
{
    public class CustomerProfile
    {
        [Key]
        public int Id { get; set; } // Ensure this property is correctly set

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty; // Initialize with default value

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty; // Initialize with default value

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty; // Initialize with default value

        [StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty; // Initialize with default value
    }
}
