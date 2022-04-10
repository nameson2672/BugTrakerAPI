using System.ComponentModel.DataAnnotations;

namespace BugTrakerAPI.ViewModel
{
    public class TokenRequest
    {
        [Required]
        public string Token { get; set; }

         [Required]
        public string RefreshToken { get; set; }
    }
}