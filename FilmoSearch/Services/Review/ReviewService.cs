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

        public IEnumerable<ReviewDto> GetAll()
        {
            return _repository.GetAll();
        }

        public ReviewDto GetById(Guid id)
        {
            ReviewDto review = _repository.GetById(id);
            if (review != null)
            {
                return _repository.GetById(id);
            }
            return null;
        }

        public ReviewDto Create(ReviewDto reviewToCreate)
        {
            bool isReviewCreated = _repository.Create(reviewToCreate);
            if (isReviewCreated)
            {
                return reviewToCreate;
            }
            return null;
        }

        public bool AddToFilm(ReviewDto reviewToCreate, Guid filmId)
        {
            bool isAddedToFilm = _repository.AddToFilm(reviewToCreate, filmId);
            if (isAddedToFilm)
            {
                return true;
            }
            return false;
        }

        public ReviewDto Update(ReviewDto reviewToUpdate)
        {
            bool isReviewUpdated = _repository.Update(reviewToUpdate);
            if (isReviewUpdated)
            {
                return reviewToUpdate;
            }
            return null;
        }

        public bool Delete(Guid id)
        {
            bool isReviewDeleted = _repository.Delete(id);
            if (isReviewDeleted)
            {
                return true;
            }
            return false;
        }
    }
}
