using System.ComponentModel.DataAnnotations;

namespace BugTrakerAPI.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string Password {get; set;}
        [Required]
        public bool RememberMe {get; set;}
    }
}