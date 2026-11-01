


namespace SurveyBasket.API.Services
{
    public interface IPollService
    {
        public Task<IEnumerable<Poll>> GetAllAsync();
        public Task<Poll?> GetAsync(int id);
        public Task<Poll> AddAsync(Poll poll);
        public Task<bool> UpdateAsync(int id, Poll poll);

        public Task<bool> DeleteAsync(int id);



    }
}
