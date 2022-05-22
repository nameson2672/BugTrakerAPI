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
        public string FirstName { get; set; }
        public string LastName {get; set;}
        public string? AvatarLink {get; set;}
        public ICollection<TeamMembers> teamMembers {get; set;}
        public ICollection<TeamAdmin> teamAdmin { get; set; }
        public ICollection<Team> team {get;set;}
        //public ICollection<TeamCreater> teamCreaters  { get; set; }
        
        
        
        
    }
}
