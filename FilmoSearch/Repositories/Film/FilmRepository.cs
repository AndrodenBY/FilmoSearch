using FilmoSearch.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FilmoSearch.Repositories.Film
{
    public class FilmRepository : IFilmoSearchRepository<FilmDto>
    {
        private ApplicationContext _context;
        public FilmRepository(ApplicationContext context) { _context = context; }

        public IEnumerable<FilmDto> GetAll()
        {
            try
            {
                return _context.Films
                    .Select(film => new FilmDto(
                        film.Id,
                        film.Title,
                        film.Reviews.Select(review => new ReviewDto(review.Id, review.Title, review.Description, review.Stars, null)).ToList(),
                        film.Actors.Select(actor => new ActorDto(actor.Id, actor.FirstName, actor.LastName, null)).ToList()
                     )).ToList();                
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in GetAll method: {ex.Message}");
                return null;
            }
        }

        public FilmDto GetById([FromBody] Guid id)
        {
            try
            {
                return _context.Films
                .Include(a => a.Actors).Include(r => r.Reviews)
                .Where(f => f.Id == id)
                .Select(f => new FilmDto(
                        f.Id,
                        f.Title,
                        f.Reviews.Select(review => new ReviewDto(review.Id, review.Title, review.Description, review.Stars, null)).ToList(),
                        f.Actors.Select(actor => new ActorDto(actor.Id, actor.FirstName, actor.LastName, null)).ToList()
                )).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in GetById method: {ex.Message}");
                return null;
            }
        }

        public bool Create(FilmDto filmToCreate)
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
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Create method: {ex.Message}");
                return false;
            }
        }

        public bool AddActor(Guid filmId, Guid actorId)
        {
            try
            {
                Models.Film film = _context.Films.Include(a => a.Actors).FirstOrDefault(f => f.Id == filmId);
                Models.Actor actorToAdd = film?.Actors.FirstOrDefault(a => a.Id == actorId);
                film?.Actors.Add(actorToAdd);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in AddActor method: {ex.Message}");
                return false;
            }
        }

        public bool AddReview(Guid filmId, Guid reviewId)
        {
            try
            {
                Models.Film film = _context.Films.Include(x => x.Reviews).FirstOrDefault(f => f.Id == filmId);
                Models.Review reviewToAdd = film?.Reviews.FirstOrDefault(r => r.Id == reviewId);
                film?.Reviews.Add(reviewToAdd);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in AddReview method: {ex.Message}");
                return false;
            }
        }     

        public bool Update(FilmDto filmToUpdate)
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
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Update method: {ex.Message}");
                return false;
            }
        }

        public bool RemoveActor(Guid filmId, Guid actorId)
        {
            try
            {
                Models.Film film = _context.Films.Include(f => f.Actors).FirstOrDefault(f => f.Id == filmId);
                Models.Actor actorToRemove = film?.Actors.FirstOrDefault(a => a.Id == actorId);                
                film?.Actors.Remove(actorToRemove);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in RemoveActor method: {ex.Message}");
                return false;
            }
        }

        public bool RemoveReview(Guid filmId, Guid reviewId)
        {
            try
            {
                Models.Film film = _context.Films.Include(f => f.Reviews).FirstOrDefault(f => f.Id == filmId);
                Models.Review reviewToRemove = film?.Reviews.FirstOrDefault(a => a.Id == reviewId);                
                film?.Reviews.Remove(reviewToRemove);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in RemoveReview method: {ex.Message}");
                return false;
            }
        }

        public bool Delete(Guid id)
        {
            try
            {
                Models.Film filmToDelete = _context.Films.FirstOrDefault(a => a.Id == id);
                _context.Films.Remove(filmToDelete);
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
