using System.ComponentModel.DataAnnotations;

namespace BugTrakerAPI.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [DataType("Email")]
        public string Email {get; set;}
        [Required]
        public string Password {get; set;}
        [Required]
        public bool RememberMe {get; set;}
    }
}