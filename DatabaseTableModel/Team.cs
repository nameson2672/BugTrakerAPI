using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BugTrakerAPI.Model;

namespace BugTrakerAPI.DatabaseTableModel
{
    public class Team
    {
        [Key]
        public string teamId {get; set;}
        [Required]
        public string teamName {get; set;}
        [Required]
        public string description {get; set;}
        [Required]
        public string coverImageLink {get; set;}
        public string teamAvatar {get; set;}
        public string createrId {get; set;}
         public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<TeamMembers> teamMembers {get; set;}
        public ICollection<TeamAdmin> teamAdmin { get; set; }
        
        
    }
}