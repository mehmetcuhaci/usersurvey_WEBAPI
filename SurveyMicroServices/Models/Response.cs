namespace SurveyMicroServices.Models
{
    public class Response
    {
        public int ResponseID { get; set; }
        public int SurveyID { get; set; }
        public int QuestionID { get; set; }
        public int OptionID { get; set; }
        public string UserID { get; set; }
        public DateTime AnsweredAt { get; set; }
       // public bool AzureIntegration { get; set; }

        // Optional: If you have navigation properties, you can define them here
        // public Survey Survey { get; set; }
        // public Question Question { get; set; }
        // public Option Option { get; set; }
        // public AppUser User { get; set; }
    }
}
