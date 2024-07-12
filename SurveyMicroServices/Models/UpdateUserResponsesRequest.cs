using static SurveyMicroServices.Controllers.SurveyController;
using SurveyMicroServices.Dtos;
namespace SurveyMicroServices.Models
{
    public class UpdateUserResponsesRequest
    {
        public int SurveyId { get; set; }
        public string UserId { get; set; }
        public List<UserResponseDto> Responses { get; set; }
    }
}
