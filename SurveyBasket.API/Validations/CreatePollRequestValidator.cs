using FluentValidation;
using SurveyBasket.API.Contracts.Requests;
using System.Data;

namespace SurveyBasket.API.Validations
{
    public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
    {
        public CreatePollRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("The {PropertyName}  must have a number");
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} can not be empty")
                .Length(3,13);
            RuleFor(x => x.BirthDate)
                .Must(BeMoreThan18Years)
                .When(x => x.BirthDate.HasValue)
               //x => DateTime.Today > x.AddYears(18)
               ;

        }

        private bool BeMoreThan18Years(DateTime? dd)
        {
            return DateTime.Today > dd!.Value.AddYears(18);
        }
    }
}
