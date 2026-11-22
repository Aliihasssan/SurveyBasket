namespace SurveyBasket.API.Contracts.Questions
{
    public record QuestionResponse
    (
        int id ,
        string Content,
        IEnumerable<Answer> Answers
        );
}
