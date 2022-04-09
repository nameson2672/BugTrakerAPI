using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TodoApp.Models;

namespace BugTrakerAPI.Model
{
    public class UserInfoModel : IdentityUser
    {
        public string Id {set; get;}
        public string Name { get; set; } = String.Empty;
        public DbSet<RefreshToken> RefreshTokens {get;set;}
    }
}
