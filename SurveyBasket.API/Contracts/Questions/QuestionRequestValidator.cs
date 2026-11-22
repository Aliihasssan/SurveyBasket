namespace SurveyBasket.API.Contracts.Questions
{
    public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
    {
        public QuestionRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .Length(3, 1000);

            RuleFor(x => x.Answers)
                .NotEmpty();

            RuleFor(x => x.Answers)
                .Must(x => x.Count > 1)
                .WithMessage("Question should have at least 2 answers")
                .When(x => x.Answers != null);

            RuleFor(x => x.Answers)
                .Must(x => x.Distinct().Count() == x.Count())
                .WithMessage("Question should not have duplicated answer")
                .When(x => x.Answers != null);
        }
    }
}
