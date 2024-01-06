using System.ComponentModel.DataAnnotations;

namespace HR.Models
{
    public class SignUpModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Only letters, numbers, and underscores are allowed in the username")]
        
        public string Username { get; set; }
        

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

      
    }
}
