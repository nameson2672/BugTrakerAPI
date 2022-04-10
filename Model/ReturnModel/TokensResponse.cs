using System.ComponentModel.DataAnnotations;

namespace BugTrakerAPI.Model.ReturnModel
{
    public class TokensResponse
    {
        [Required]
        public string Token { get; set; }

         [Required]
        public string RefreshToken { get; set; }
    }
}