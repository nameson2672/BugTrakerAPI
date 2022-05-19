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
        public string UserId { get; set; }
        [Required]
        public string TeamId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual UserInfoModel userModel { get; set; }
        [ForeignKey(nameof(TeamId))]
        public virtual Team teamModel { get; set; }
    }
}