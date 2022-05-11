using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using BugTrakerAPI.Model;

namespace BugTrakerAPI.Model
{
    public class UserInfoModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName {get; set;}

       
    }
}
