using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        // The user’s unique login name
        [Required]
        public string Username { get; set; } = string.Empty;

        // Plain-text password for simplicity 
        [Required]
        public string Password { get; set; } = string.Empty;

        // Could be "Admin", "Staff", or "Customer"
        [Required]
        public string Role { get; set; } = "Customer";
    }
}
