using SurveyBasket.API.Contracts.Questions;

namespace SurveyBasket.API.Services
{
    public interface IQuestionService
    {
        public Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId,CancellationToken cancellationToken = default);
        public Task<Result<QuestionResponse>> GetAsync (int pollId,int id , CancellationToken cancellationToken = default);
        public Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default);

        public Task<Result> ToggleStatusAsync (int pollId,int id ,CancellationToken cancellationToken = default);
    }
}
