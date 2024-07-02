namespace SurveyMicroServices.Models
{
    public class Survey
    {
        public int SurveyID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public Guid CreatedBy { get; set; }
        public byte Status { get; set; }
        public ICollection<Question> Questions { get; set; }
        //public ICollection<UserSurvey> UserSurveys { get; set; }
    }

    public class Question
    {
        public int QuestionID { get; set; }
        public int SurveyID { get; set; }
        public string Text { get; set; }
        public ICollection<Option> Options { get; set; }
    }

    public class Option
    {
        public int OptionID { get; set; }
        public int QuestionID { get; set; }
        public string Text { get; set; }
    }
}
