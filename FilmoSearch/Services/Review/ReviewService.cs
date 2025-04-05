using FilmoSearch.DTO;
using FilmoSearch.Repositories.Review;

namespace FilmoSearch.Services.Review
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;
        public ReviewService(IReviewRepository repository) { _repository = repository; }

        public async Task<IEnumerable<ReviewDto>?> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ReviewDto?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ReviewDto?> CreateAsync(ReviewDto reviewToCreate)
        {
            return await _repository.CreateAsync(reviewToCreate) ? reviewToCreate : null;
        }

        public async Task<bool> AddToFilmAsync(ReviewDto reviewToCreate, Guid filmId)
        {
            return await _repository.AddToFilmAsync(reviewToCreate, filmId) ? true : false;
        }

        public async Task<ReviewDto?> UpdateAsync(ReviewDto reviewToUpdate)
        {
            return await _repository.UpdateAsync(reviewToUpdate) ? reviewToUpdate : null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id) ? true : false;
        }
    }
}
