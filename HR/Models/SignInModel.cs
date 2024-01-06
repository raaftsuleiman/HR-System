using System.ComponentModel.DataAnnotations;

namespace HR.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool RemeberMe { get; set; }
    }
}
