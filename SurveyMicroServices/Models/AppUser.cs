using Microsoft.AspNetCore.Identity;

namespace SurveyMicroServices.Models
{
    public sealed class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName  =>string.Join(" ",FirstName,LastName);

        public ICollection<UserSurvey> UserSurveys { get; set; }

    }
}
