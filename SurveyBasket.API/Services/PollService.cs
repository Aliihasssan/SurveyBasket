using System.Threading.Tasks;

namespace SurveyBasket.API.Services
{
    public class PollService(ApplicationDbContext context) : IPollService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Poll>> GetAllAsync()
        {
            return await _context.Polls.AsNoTracking().ToListAsync();
        }

        public async Task<Poll?> GetAsync(int id)
        {
            return await _context.Polls.FindAsync(id);

        }

        public async Task<Poll> AddAsync(Poll poll)
        {
            await _context.Polls.AddAsync(poll);
            await _context.SaveChangesAsync();
            return poll;
        }

        public async Task<bool> UpdateAsync(int id, Poll poll)
        {
            var currnetPoll =await GetAsync(id);

            if (currnetPoll == null)
                return false;

            currnetPoll.Title = poll.Title;
            currnetPoll.Summary = poll.Summary;
            currnetPoll.StartsAt = poll.StartsAt;
            currnetPoll.EndsAt = poll.EndsAt;

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            Poll? poll = await GetAsync(id);

            if (poll == null)
                return false;

            _context.Remove(poll);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

