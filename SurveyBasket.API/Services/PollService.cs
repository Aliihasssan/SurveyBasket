using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading;
using System.Threading.Tasks;

namespace SurveyBasket.API.Services
{
    public class PollService(ApplicationDbContext context) : IPollService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var polls =  await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);
            return Result.Success(polls.Adapt<IEnumerable<PollResponse>>());
        }

        public async Task<Result<PollResponse>> GetAsync(int id ,CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);

            return poll is null ? Result.Failure<PollResponse>(PollErrors.PollNotFound) : Result.Success(poll.Adapt<PollResponse>());  

        }


        public async Task<Result<PollResponse>> AddAsync(PollRequest poll, CancellationToken cancellationToken = default)
        {
            var isExisitingTitle = await _context.Polls.AnyAsync(x => x.Title == poll.Title, cancellationToken: cancellationToken);
            if (isExisitingTitle)
                return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);
            var currentPoll = poll.Adapt<Poll>();
            await _context.Polls.AddAsync(currentPoll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(currentPoll.Adapt<PollResponse>());
        }

        public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default)
        {
            var currnetPoll =await _context.Polls.FindAsync(id, cancellationToken);

            if (currnetPoll == null)
                return Result.Failure(PollErrors.PollNotFound);

            currnetPoll.Title = poll.Title;
            currnetPoll.Summary = poll.Summary;
            currnetPoll.StartsAt = poll.StartsAt;
            currnetPoll.EndsAt = poll.EndsAt;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await GetAsync(id, cancellationToken);

            if (poll.IsFailure)
                return Result.Failure(PollErrors.PollNotFound);

            _context.Remove(poll);
            await _context.SaveChangesAsync( cancellationToken);
            return Result.Success();
        }

        public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);
            if (currentPoll is null)
                return Result.Failure(PollErrors.PollNotFound);
            currentPoll!.IsPublished = ! currentPoll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    
    }
}

