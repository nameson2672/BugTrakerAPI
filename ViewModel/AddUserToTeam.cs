using System.ComponentModel.DataAnnotations;

namespace BugTrakerAPI.ViewModel
{
    public class AddUserToTeam
    {
        [Required]
        public string teamId {get; set;}
        [Required]
        public string userId { get; set; }
        
        
    }
    
}