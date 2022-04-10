using System.ComponentModel.DataAnnotations;

namespace BugTrakerAPI.Model.ReturnModel
{
    public class TokensResponse
    {   
        [Required]
        public bool success {get; set;}
        [Required]
        public string? Token { get; set; }

         [Required]
        public string? RefreshToken { get; set; }
        public List<string>? errors {get; set;}
    }
}