using FilmoSearch.DTO;
using FilmoSearch.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FilmoSearch.Repositories.Review
{
    public class ReviewRepository : IFilmoSearchRepository<ReviewDto>
    {
        private ApplicationContext _context;
        public ReviewRepository(ApplicationContext context) { _context = context; }

        public IEnumerable<ReviewDto> GetAll()
        {
            try
            {
                return _context.Reviews.Select(review => new ReviewDto(
                    review.Id,
                    review.Title,
                    review.Description,
                    review.Stars,
                    new FilmDto(review.Film.Id, review.Film.Title, null, null)
                    )).ToList();                
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in GetAll method: {ex.Message}");
                return null;
            }
        }

        public ReviewDto GetById(Guid id)
        {
            try
            {
                return _context.Reviews.Where(a => a.Id == id)
                    .Select(review => new ReviewDto(
                        review.Id,
                        review.Title,
                        review.Description,
                        review.Stars,
                        new FilmDto(review.Film.Id, review.Film.Title, null, null)
                        )).FirstOrDefault(r => r.Id == id);
                //return _context.Reviews.FirstOrDefault(r => r.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in GetById method: {ex.Message}");
                return null;
            }
        }

        public bool Create(ReviewDto reviewToCreate)
        {
            Models.Review newReview = new Models.Review
            {
                Id = reviewToCreate.Id,
                Title = reviewToCreate.Title,
                Description = reviewToCreate.Description,
                Stars = reviewToCreate.Stars
            };
            try
            {
                _context.Reviews.Add(newReview);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Create method: {ex.Message}");
                return false;
            }
        }

        public bool AddToFilm(ReviewDto reviewToCreate, Guid filmId)
        {
            Models.Review newReview = new Models.Review
            {
                Id = reviewToCreate.Id,
                Title = reviewToCreate.Title,
                Description = reviewToCreate.Description,
                Stars = reviewToCreate.Stars
            };
            try
            {
                _context.Reviews.Add(newReview);
                Models.Film film = _context.Films.Find(filmId);
                newReview.FilmId = filmId;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in AddToFilm method: {ex.Message}");
                return false;
            }
        }

        public bool Update(ReviewDto reviewToUpdate)
        {
            Models.Review updateReview = new Models.Review
            {
                Id = reviewToUpdate.Id,
                Title = reviewToUpdate.Title,
                Description = reviewToUpdate.Description,
                Stars = reviewToUpdate.Stars
            };
            try
            {
                _context.Reviews.Update(updateReview);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Update method: {ex.Message}");
                return false;
            }
        }

        public bool Delete(Guid id)
        {
            try
            {
                Models.Review reviewToDelete = _context.Reviews.FirstOrDefault(a => a.Id == id);
                _context.Reviews.Remove(reviewToDelete);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Delete method: {ex.Message}");
                return false;
            }
        }
    }
}
