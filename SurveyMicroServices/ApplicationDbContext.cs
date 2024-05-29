using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyMicroServices;
using SurveyMicroServices.Models;

namespace SurveyMicroServices
{
    public sealed class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid,IdentityUserClaim<Guid>,AppUserRole,IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUserRole>().HasKey(x=>new { x.UserId, x.RoleId }); //composite key

            builder.Ignore<IdentityUserLogin<Guid>>();
            builder.Ignore<IdentityUserToken<Guid>>();
            builder.Ignore<IdentityUserClaim<Guid>>();
            builder.Ignore<IdentityRoleClaim<Guid>>();
        }
    }
}
