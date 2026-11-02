


namespace SurveyBasket.API.Services
{
    public interface IPollService
    {
        public Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken =default);
        public Task<Poll?> GetAsync(int id,CancellationToken cancellationToken = default);
        public Task<Poll> AddAsync(Poll poll);
        public Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken = default);

        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);



    }
}
