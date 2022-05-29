using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BugTrakerAPI.Model;

namespace BugTrakerAPI.DatabaseTableModel
{
    public class Team
    {
        [Key]
        public string teamId {get; set;} = Guid.NewGuid().ToString();
        [Required]
        public string teamName {get; set;}
        public string workingOn {get; set;}
        public string mainFunctions {get; set;}
        public string description {get; set;}
        public string? coverImageLink {get; set;}
        public string? teamAvatar {get; set;}
        public string createrId {get; set;}
         public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<TeamMembers> teamMembers {get; set;}
        
        public virtual UserInfoModel userModel {get; set;}
        
    }
}