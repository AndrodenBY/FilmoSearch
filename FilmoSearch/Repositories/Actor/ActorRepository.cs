using FilmoSearch.DTO;
using FilmoSearch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FilmoSearch.Repositories.Actor
{
    public class ActorRepository : IFilmoSearchRepository<ActorDto>
    {
        private readonly ApplicationContext _context;
        public ActorRepository(ApplicationContext context) { _context = context; }

        public async Task<IEnumerable<ActorDto>> GetAllAsync()
        {
            try
            {
                return await _context.Actors.Select(actor => new ActorDto(
                    actor.Id,
                    actor.FirstName,
                    actor.LastName,
                    actor.Films.Select(film => new FilmDto(film.Id, film.Title, null, null)).ToList()
                )).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in GetAll method: {ex.Message}");
                return null;
            }
        }

        public async Task<ActorDto> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Actors
                    .Include(a => a.Films)
                    .Where(a => a.Id == id)
                    .Select(a => new ActorDto(
                        a.Id,
                        a.FirstName,
                        a.LastName,
                        a.Films.Select(film => new FilmDto(film.Id, film.Title, null, null)).ToList()
                    ))
                    .FirstOrDefaultAsync();                
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in GetById method: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateAsync(ActorDto actorToCreate)
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
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Create method: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> AddFilmAsync(Guid actorId, Guid filmId)
        {
            try
            {
                Models.Actor actor = await _context.Actors.Include(f => f.Films).FirstOrDefaultAsync(a => a.Id == actorId);
                Models.Film filmToAdd = actor?.Films.FirstOrDefault(f => f.Id == filmId);
                actor?.Films.Add(filmToAdd);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"An error occurred in AddFilm method: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(ActorDto actorToUpdate)
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
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in Update method: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveFilmAsync(Guid actorId, Guid filmId)
        {
            try
            {
                Models.Actor actor = await _context.Actors.Include(f => f.Films).FirstOrDefaultAsync(a => a.Id == actorId);
                Models.Film filmToRemove = actor?.Films.FirstOrDefault(f => f.Id == filmId);
                actor?.Films.Remove(filmToRemove);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred in RemoveFilm method: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                Models.Actor actorToDelete = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id);
                _context.Actors.Remove(actorToDelete);
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
