using FluentValidation;
using System.Data;

namespace SurveyBasket.API.Contracts.Polls
{
    public class PollRequestValidator : AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
           RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("{PropertyName} can not be empty")
                .Length(3,100);

            RuleFor(x => x.Summary).Length(3,1500);

            RuleFor(x => x.StartsAt)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(x => x)
               //. Must(x => x.EndsAt >= x.StartsAt)
               .Must(HasValidDate)
               .WithName(nameof(PollRequest.EndsAt))
                .WithMessage("End time is less than start time");
        }
        private bool HasValidDate(PollRequest pollRequest)
        {
            return pollRequest.StartsAt <= pollRequest.EndsAt;
        }

    }
}
