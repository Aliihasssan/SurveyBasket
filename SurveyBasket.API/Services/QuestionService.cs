using SurveyBasket.API.Contracts.Questions;

namespace SurveyBasket.API.Services
{
    public class QuestionService(ApplicationDbContext context) : IQuestionService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var doesPollExist =await _context.Polls.AnyAsync(x => x.Id == pollId,cancellationToken);

            if (!doesPollExist)
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);
            var doesQuestionExist = await _context.Question.AnyAsync(x => x.Content == request.Content && x.PollId == pollId,cancellationToken);

            if (doesQuestionExist)
                return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestion);
            var question = request.Adapt<Question>(); 
            question.PollId = pollId;

            request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));

            await _context.AddAsync(question,cancellationToken);
            await _context.SaveChangesAsync();

            return Result.Success(question.Adapt<QuestionResponse>());
        }
    }
}
