using FilmoSearch.DTO;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq;

namespace FilmoSearch.Repositories.Review
{
    public class ReviewRepository : IFilmoSearchRepository<ReviewDto>
    {
        private readonly ApplicationContext _context;
        public ReviewRepository(ApplicationContext context) { _context = context; }

        public async Task<IEnumerable<ReviewDto>?> GetAllAsync()
        {
            try
            {
                return await _context.Reviews.Select(review => new ReviewDto(
                    review.Id,
                    review.Title,
                    review.Description,
                    review.Stars,
                    new FilmDto(review.Film.Id, review.Film.Title, null, null)
                    )).ToListAsync();                
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in GetAll method: {ex.Message}");
                return null;
            }
        }

        public async Task<ReviewDto?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Reviews.Where(a => a.Id == id)
                    .Select(review => new ReviewDto(
                        review.Id,
                        review.Title,
                        review.Description,
                        review.Stars,
                        new FilmDto(review.Film.Id, review.Film.Title, null, null)
                        )).FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in GetById method: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateAsync(ReviewDto reviewToCreate)
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
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Create method: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddToFilmAsync(ReviewDto reviewToCreate, Guid filmId)
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
                Models.Film film = await _context.Films.FindAsync(filmId);
                newReview.FilmId = filmId;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in AddToFilm method: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(ReviewDto reviewToUpdate)
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
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Update method: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                Models.Review reviewToDelete = await _context.Reviews.FirstOrDefaultAsync(a => a.Id == id);
                _context.Reviews.Remove(reviewToDelete);
                await _context.SaveChangesAsync();
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
