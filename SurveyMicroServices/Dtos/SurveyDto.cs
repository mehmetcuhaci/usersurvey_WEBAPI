using Microsoft.Identity.Client;

namespace SurveyMicroServices.Dtos
{
    public class SurveyDto
    {
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }

    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public List<OptionDto> Options { get; set; }
    }

    public class OptionDto
    {
        public int OptionId { get; set; }
        public string Text { get; set; }
    }
}
