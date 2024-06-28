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

            builder.Entity<AppUserRole>().HasKey(x => new { x.UserId, x.RoleId });

            builder.Ignore<IdentityUserLogin<Guid>>();
            builder.Ignore<IdentityUserToken<Guid>>();
            builder.Ignore<IdentityUserClaim<Guid>>();
            builder.Ignore<IdentityRoleClaim<Guid>>();

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
