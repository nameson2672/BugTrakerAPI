using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BugTrakerAPI.Model;

namespace BugTrakerAPI.Model
{
    public class ApplicationDbContext : IdentityDbContext<UserInfoModel>
    {
         public virtual DbSet<RefreshToken> RefreshTokens {get;set;}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
           
        }

    }
}
