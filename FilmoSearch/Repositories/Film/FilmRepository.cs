using FilmoSearch.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq;

namespace FilmoSearch.Repositories.Film
{
    public class FilmRepository : IFilmoSearchRepository<FilmDto>
    {
        private readonly ApplicationContext _context;
        public FilmRepository(ApplicationContext context) { _context = context; }

        public async Task<IEnumerable<FilmDto>?> GetAllAsync()
        {
            try
            {
                return await _context.Films
                    .Select(film => new FilmDto(
                        film.Id,
                        film.Title,
                        film.Reviews.Select(review => new ReviewDto(review.Id, review.Title, review.Description, review.Stars, null)).ToList(),
                        film.Actors.Select(actor => new ActorDto(actor.Id, actor.FirstName, actor.LastName, null)).ToList()
                     )).ToListAsync();                
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in GetAll method: {ex.Message}");
                return null;
            }
        }

        public async Task<FilmDto?> GetByIdAsync([FromBody] Guid id)
        {
            try
            {
                return await _context.Films
                .Include(a => a.Actors).Include(r => r.Reviews)
                .Where(f => f.Id == id)
                .Select(f => new FilmDto(
                        f.Id,
                        f.Title,
                        f.Reviews.Select(review => new ReviewDto(review.Id, review.Title, review.Description, review.Stars, null)).ToList(),
                        f.Actors.Select(actor => new ActorDto(actor.Id, actor.FirstName, actor.LastName, null)).ToList()
                )).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in GetById method: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateAsync(FilmDto filmToCreate)
        {
            Models.Film newFilm = new Models.Film
            {
                Id = filmToCreate.Id,
                Title = filmToCreate.Title,
                Actors = filmToCreate.Actors != null ? filmToCreate.Actors.Select(actorDto => new Models.Actor
                {
                    Id = actorDto.Id,
                    FirstName = actorDto.FirstName,
                    LastName = actorDto.LastName
                }).ToList() : new List<Models.Actor>(),
                Reviews = filmToCreate.Reviews != null ? filmToCreate.Reviews.Select(reviewDto => new Models.Review
                { 
                    Id = reviewDto.Id,
                    Title = reviewDto.Title,
                    Description = reviewDto.Description,
                    Stars = reviewDto.Stars,
                }).ToList() : new List<Models.Review>()
            };
            try
            {
                _context.Films.Add(newFilm);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Create method: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddActorAsync(Guid filmId, Guid actorId)
        {
            try
            {
                Models.Film? film = await _context.Films.Include(a => a.Actors).FirstOrDefaultAsync(f => f.Id == filmId);
                Models.Actor actorToAdd = film?.Actors.FirstOrDefault(a => a.Id == actorId);
                film?.Actors.Add(actorToAdd);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in AddActor method: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddReviewAsync(Guid filmId, Guid reviewId)
        {
            try
            {
                Models.Film? film = await _context.Films.Include(x => x.Reviews).FirstOrDefaultAsync(f => f.Id == filmId);
                Models.Review reviewToAdd = film?.Reviews.FirstOrDefault(r => r.Id == reviewId);
                film?.Reviews.Add(reviewToAdd);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in AddReview method: {ex.Message}");
                return false;
            }
        }     

        public async Task<bool> UpdateAsync(FilmDto filmToUpdate)
        {
            Models.Film updateFilm = new Models.Film
            {
                Id = filmToUpdate.Id,
                Title = filmToUpdate.Title,
                Actors = filmToUpdate.Actors != null ? filmToUpdate.Actors
                         .Select(a => new Models.Actor { Id = a.Id, FirstName = a.FirstName, LastName = a.LastName})
                         .ToList() : new List<Models.Actor>(),
                Reviews = filmToUpdate.Reviews != null ? filmToUpdate.Reviews
                         .Select(r => new Models.Review { Id = r.Id, Title = r.Title, Description = r.Description, Stars = r.Stars })
                         .ToList() : new List<Models.Review>()
            };
            try
            {
                _context.Films.Update(updateFilm);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Update method: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveActorAsync(Guid filmId, Guid actorId)
        {
            try
            {
                Models.Film film = await _context.Films.Include(f => f.Actors).FirstOrDefaultAsync(f => f.Id == filmId);
                Models.Actor actorToRemove = film?.Actors.FirstOrDefault(a => a.Id == actorId);                
                film?.Actors.Remove(actorToRemove);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in RemoveActor method: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveReviewAsync(Guid filmId, Guid reviewId)
        {
            try
            {
                Models.Film film = await _context.Films.Include(f => f.Reviews).FirstOrDefaultAsync(f => f.Id == filmId);
                Models.Review reviewToRemove = film?.Reviews.FirstOrDefault(a => a.Id == reviewId);                
                film?.Reviews.Remove(reviewToRemove);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in RemoveReview method: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                Models.Film filmToDelete = await _context.Films.FirstOrDefaultAsync(a => a.Id == id);
                _context.Films.Remove(filmToDelete);
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
