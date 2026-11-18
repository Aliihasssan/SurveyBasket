namespace SurveyBasket.API.Services
{
    public interface IPollService
    {
        public Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken =default);
        public Task<Result<PollResponse>> GetAsync(int id,CancellationToken cancellationToken = default);
        public Task<Result<PollResponse>> AddAsync(PollRequest poll,CancellationToken cancellationToken=default);
        public Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default);

        public Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);

        public Task<Result> TogglePublishStatusAsync(int id,CancellationToken cancellationToken=default);

    }
}
