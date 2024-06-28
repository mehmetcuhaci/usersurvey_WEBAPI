namespace SurveyMicroServices.Models
{
    public class UserSurvey
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public int SurveyID { get; set; }
        public Survey Survey { get; set; }
    }
}
