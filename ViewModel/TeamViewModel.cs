using System.ComponentModel.DataAnnotations;

namespace BugTrakerAPI.ViewModel
{
    public class TeamViewModel
    {
        [Required(ErrorMessage = "Team name required")]

        public string teamName { get; set; }
        public string description { get; set; }
        public string workingOn { get; set; }
        public string mainFunctions { get; set; }

    }
    public class TeamUpdateViewModle
    {
        [Required(ErrorMessage = "Please Provide the Team Id")]
        public string teamId { get; set; }
        public string teamName { get; set; }
        public string? description { get; set; }
        public string? workingOn { get; set; }
        public string? mainFunctions { get; set; }

    }
}