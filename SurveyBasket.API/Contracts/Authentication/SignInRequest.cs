namespace SurveyBasket.API.Contracts.Authentication
{
    public record SignInRequest
        (
            string Email,
            string Password
        );
    
}
