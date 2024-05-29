namespace SurveyMicroServices.Dtos
{
    public sealed record LoginDto(
        string UserNameorOrEmail,
        string Password
        
        );
   
}
