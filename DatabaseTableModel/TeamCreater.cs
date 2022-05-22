using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BugTrakerAPI.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BugTrakerAPI.DatabaseTableModel
{
    [Keyless]
    public class TeamCreater
    {
        [Required]
        public string teamId { get; set; }
        public string userId { get; set; }
         [ForeignKey(nameof(userId))]
        public virtual UserViewModel UserModel {get; set;}
         [ForeignKey(nameof(teamId))]
        public virtual Team teamModel { get; set; }
        

        
        
    }
}