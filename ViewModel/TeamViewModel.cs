using System.ComponentModel.DataAnnotations;

namespace BugTrakerAPI.ViewModel
{
    public class TeamViewModel
    {
        [Required(ErrorMessage = "Password is required")]

        public string teamName {get; set;}
        public string description {get; set;}
        public string workingOn {get; set;}
        public string mainFunctions {get; set;}

    }
}