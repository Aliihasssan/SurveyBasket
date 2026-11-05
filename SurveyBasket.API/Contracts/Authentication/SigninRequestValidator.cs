namespace SurveyBasket.API.Contracts.Authentication
{
    public class SigninRequestValidator: AbstractValidator<SignInRequest>
    {
        public SigninRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
