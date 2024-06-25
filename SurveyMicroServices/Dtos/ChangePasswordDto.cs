namespace SurveyMicroServices.Dtos
{
    public sealed record ChangePasswordDto(
        
        
        string email,
        string currentPassword,
        string newPassword
        
        
        );
    
}
