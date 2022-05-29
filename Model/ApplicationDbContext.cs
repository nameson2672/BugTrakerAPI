using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BugTrakerAPI.Model;
using BugTrakerAPI.DatabaseTableModel;

namespace BugTrakerAPI.Model
{
    public class ApplicationDbContext : IdentityDbContext<UserInfoModel>
    {
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<TeamMembers> TeamMembers { get; set; }
       
        
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // added to encounter AspRole table null key error during migration
            modelBuilder.Entity<TeamMembers>().HasKey(l => new { l.userId, l.teamId }); // FK for team member table

            // Making many-to-many relation in to create teammember table 
            modelBuilder.Entity<TeamMembers>().HasOne(u => u.userModel).WithMany(Tm => Tm.teamMembers).HasForeignKey(Uf => Uf.userId);
            modelBuilder.Entity<TeamMembers>().HasOne(T => T.teamModel).WithMany(Tm => Tm.teamMembers).HasForeignKey(Tf => Tf.teamId);


            //modelBuilder.Entity<Team>().HasKey(l=> new {l.createrId});
            modelBuilder.Entity<Team>().HasOne(user => user.userModel).WithMany(tc => tc.team).HasForeignKey(tf =>tf.createrId).OnDelete(DeleteBehavior.NoAction);

        }


    }

}

