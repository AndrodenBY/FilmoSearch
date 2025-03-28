using FilmoSearch.DTO;
using FilmoSearch.Models;
using FilmoSearch.Repositories.Review;
using FilmoSearch.Services;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FilmoSearch.Services.Review
{
    public class ReviewService : IFilmoSearchService<ReviewDto>
    {
        private readonly ReviewRepository _repository;
        public ReviewService(ReviewRepository repository) { _repository = repository; }

        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ReviewDto> GetByIdAsync(Guid id)
        {
            ReviewDto review = await _repository.GetByIdAsync(id);
            if (review != null)
            {
                return review;
            }
            return null;
        }

        public async Task<ReviewDto> CreateAsync(ReviewDto reviewToCreate)
        {
            bool isReviewCreated = await _repository.CreateAsync(reviewToCreate);
            if (isReviewCreated)
            {
                return reviewToCreate;
            }
            return null;
        }

        public async Task<bool> AddToFilmAsync(ReviewDto reviewToCreate, Guid filmId)
        {
            bool isAddedToFilm = await _repository.AddToFilmAsync(reviewToCreate, filmId);
            if (isAddedToFilm)
            {
                return true;
            }
            return false;
        }

        public async Task<ReviewDto> UpdateAsync(ReviewDto reviewToUpdate)
        {
            bool isReviewUpdated = await _repository.UpdateAsync(reviewToUpdate);
            if (isReviewUpdated)
            {
                return reviewToUpdate;
            }
            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool isReviewDeleted = await _repository.DeleteAsync(id);
            if (isReviewDeleted)
            {
                return true;
            }
            return false;
        }
    }
}
