using System.ComponentModel.DataAnnotations;

namespace BugTrakerAPI.Model
{
    public class MailModel
    {
        [Required, Display(Name = "Your name")]  
        public string toname { get; set; }  
        [Required, Display(Name = "Your email"), EmailAddress]  
        public string toemail { get; set; }  
        [Required]  
        public string subject { get; set; }
        [Required]  
        public string message { get; set; }  
    }
}