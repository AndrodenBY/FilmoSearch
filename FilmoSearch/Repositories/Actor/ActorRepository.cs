using FilmoSearch.DTO;
using FilmoSearch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FilmoSearch.Repositories.Actor
{
    public class ActorRepository : IFilmoSearchRepository<ActorDto>
    {
        private ApplicationContext _context;
        public ActorRepository(ApplicationContext context) { _context = context; }

        public IEnumerable<ActorDto> GetAll()
        {
            try
            {                
                return _context.Actors.Select(actor => new ActorDto(
                    actor.Id,
                    actor.FirstName,
                    actor.LastName,
                    actor.Films.Select(film => new FilmDto(film.Id, film.Title, null, null)).ToList()
                )).ToList();                
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in GetAll method: {ex.Message}");
                return null;
            }
        }

        public ActorDto GetById(Guid id)
        {
            try
            {
                return _context.Actors
                    .Include(a => a.Films)
                    .Where(a => a.Id == id)
                    .Select(a => new ActorDto(
                        a.Id,
                        a.FirstName,
                        a.LastName,
                        a.Films.Select(film => new FilmDto(film.Id, film.Title, null, null)).ToList()
                    ))
                    .FirstOrDefault();                
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in GetById method: {ex.Message}");
                return null;
            }
        }

        public bool Create(ActorDto actorToCreate)
        {
            Models.Actor newActor = new Models.Actor
            {
                Id = actorToCreate.Id,
                FirstName = actorToCreate.FirstName,
                LastName = actorToCreate.LastName,
                Films = actorToCreate.Films != null ? actorToCreate.Films.Select(filmDto => new Models.Film
                {
                    Id = filmDto.Id,
                    Title = filmDto.Title
                }).ToList() : new List<Models.Film>()
            };
            try
            {
                _context.Actors.Add(newActor);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Create method: {ex.Message}");
                return false;
            }
        }
        
        public bool AddFilm(Guid actorId, Guid filmId)
        {
            try
            {
                Models.Actor actor = _context.Actors.Include(f => f.Films).FirstOrDefault(a => a.Id == actorId);
                Models.Film filmToAdd = actor?.Films.FirstOrDefault(f => f.Id == filmId);
                actor?.Films.Add(filmToAdd);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"An error occurred in AddFilm method: {ex.Message}");
                return false;
            }
        }

        public bool Update(ActorDto actorToUpdate)
        {
            Models.Actor updateActor = new Models.Actor
            {
                Id = actorToUpdate.Id,
                FirstName = actorToUpdate.FirstName,
                LastName = actorToUpdate.LastName,
                Films = actorToUpdate.Films != null ? actorToUpdate.Films
                        .Select(f => new Models.Film { Id = f.Id, Title = f.Title })
                        .ToList() : new List<Models.Film>()
            };
            try
            {
                _context.Actors.Update(updateActor);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Update method: {ex.Message}");
                return false;
            }
        }

        public bool RemoveFilm(Guid actorId, Guid filmId)
        {
            try
            {
                Models.Actor actor = _context.Actors.Include(f => f.Films).FirstOrDefault(a => a.Id == actorId);
                Models.Film filmToRemove = actor?.Films.FirstOrDefault(f => f.Id == filmId);
                actor?.Films.Remove(filmToRemove);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in RemoveFilm method: {ex.Message}");
                return false;
            }
        }

        public bool Delete(Guid id)
        {
            try
            {
                Models.Actor actorToDelete = _context.Actors.FirstOrDefault(a => a.Id == id);
                _context.Actors.Remove(actorToDelete);
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
