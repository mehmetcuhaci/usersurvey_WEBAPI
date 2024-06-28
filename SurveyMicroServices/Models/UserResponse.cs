using Azure;
using System.Collections.Generic;

namespace SurveyMicroServices.Models
{
    public class UserResponse
    {
        public int UserResponseId { get; set; }
        public int SurveyId { get; set; }
        public string UserId { get; set; }

        // Navigation properties
        public Survey Survey{ get; set; }
        public List<Response> Responses { get; set; }
    }
}
