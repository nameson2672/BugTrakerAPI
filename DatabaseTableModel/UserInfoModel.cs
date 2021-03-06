using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using BugTrakerAPI.Model;
using BugTrakerAPI.DatabaseTableModel;
using System.Collections.Generic;

namespace BugTrakerAPI.Model
{
    public class UserInfoModel : IdentityUser
    {
        public string Name {get; set;}
        public string? AvatarLink {get; set;}
        public virtual ICollection<TeamMembers> teamMembers {get; set;}
        public virtual ICollection<Team> team {get;set;}
        
    }
}
