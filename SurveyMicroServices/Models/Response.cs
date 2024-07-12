namespace SurveyMicroServices.Models
{
    public class Response
    {
        public int ResponseID { get; set; }
        public int SurveyID { get; set; }
        public int QuestionID { get; set; }
        public int OptionID { get; set; }
        public  Guid UserID { get; set; }
        public DateTime AnsweredAt { get; set; }
        public Question Question { get; set; } 
       // public string ResponseText { get; set; }
    }
}
