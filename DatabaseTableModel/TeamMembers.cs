using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BugTrakerAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace BugTrakerAPI.DatabaseTableModel
{
    [Keyless]
    public class TeamMembers
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public string teamId { get; set; }
        public bool isUserAdmin {get; set;}
        [ForeignKey(nameof(userId))]
        public virtual UserInfoModel userModel { get; set; }
        [ForeignKey(nameof(teamId))]
        public virtual Team teamModel { get; set; }
    }
}