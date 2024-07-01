using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyMicroServices.Models;
using System;

namespace SurveyMicroServices
{
    public sealed class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<UserSurvey> UserSurveys { get; set; }
        public DbSet<Response> Responses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Set the primary key for AppUserRole
            builder.Entity<AppUserRole>().HasKey(x => new { x.UserId, x.RoleId });

            // Map Identity tables to custom table names
            builder.Entity<AppUser>(entity => {
                entity.ToTable("Users");
            });

            builder.Entity<AppRole>(entity => {
                entity.ToTable("Roles");
            });

            builder.Entity<IdentityUserRole<Guid>>(entity => {
                entity.ToTable("UserRoles");
                entity.HasKey(r => new { r.UserId, r.RoleId });
            });

           

            // Custom mappings for UserSurvey
            builder.Entity<UserSurvey>()
                .HasKey(us => new { us.UserId, us.SurveyID });

            builder.Entity<UserSurvey>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSurveys)
                .HasForeignKey(us => us.UserId);

            builder.Entity<UserSurvey>()
                .HasOne(us => us.Survey)
                .WithMany(s => s.UserSurveys)
                .HasForeignKey(us => us.SurveyID);
        }
    }
}
