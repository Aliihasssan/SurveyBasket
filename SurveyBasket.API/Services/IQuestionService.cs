using SurveyBasket.API.Contracts.Questions;

namespace SurveyBasket.API.Services
{
    public interface IQuestionService
    {
        public Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request ,CancellationToken cancellationToken = default);
    }
}
