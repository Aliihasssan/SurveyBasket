namespace SurveyBasket.API.Contracts.Questions
{
    public record QuestionResponse
    (
        int Id ,
        string Content,
        ICollection<Answer> Answers
        );
}
