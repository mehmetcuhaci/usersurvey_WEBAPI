namespace SurveyMicroServices.Dtos
{
    public sealed record ChangePasswordDto(
        Guid Id,
        string CurrentPassowrd,
        string NewPassword
        
        
        );
    
}
