using FilmoSearch.DTO;
using FilmoSearch.Models;
using FilmoSearch.Services.Actor;
using FilmoSearch.Services.Film;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FilmoSearch.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly FilmService _filmService;        
        public FilmController(FilmService filmService) { _filmService = filmService; }

        [HttpGet("GetFilms")]
        public async Task<ActionResult<IEnumerable<Film>>> GetAsync()
        {
            Log.Information("Getting all films");
            return Ok(await _filmService.GetAllAsync());
        }

        [HttpGet("GetFilmById/{id}")]
        public async Task<ActionResult<FilmDto>> GetByIdAsync([FromRoute] Guid id)
        {
            Log.Information($"Getting film by ID: {id}");
            FilmDto? film = await _filmService.GetByIdAsync(id);
            if (film != null)
            {
                Log.Information($"Film found: {film}");
                return Ok(film);
            }
            Log.Warning($"Film with ID {id} not found");
            return Ok("Error"); 
        }        

        [HttpPost("AddFilm")]
        public async Task<ActionResult<FilmDto>> AddAsync(FilmDto filmToCreate)
        {
            Log.Information($"AddFilm request recieved: {filmToCreate}");
            FilmDto? film = await _filmService.CreateAsync(filmToCreate);
            if (film != null)
            {
                Log.Information($"AddFilm response: {film}");
                return Ok(film);
            }
            Log.Error("AddFilm responce: Error");
            return Ok("Error");
        }

        [HttpPost("AddFilmActor/{filmId}/{actorId}")]
        public async Task<ActionResult<FilmDto>> AddActorAsync([FromRoute] Guid filmId, [FromRoute] Guid actorId)
        {
            Log.Information($"AddFilmActor request received film with ID {filmId} and actor with ID {actorId}");
            await _filmService.AddActorAsync(filmId, actorId);
            if (actorId != Guid.Empty)
            {
                Log.Information($"AddFilmActor response: {actorId} added to film");
                return Ok("Actor added");
            }
            Log.Error("Error adding actor");
            return Ok("Error");
        }
        
        [HttpPost("AddFilmReview/{filmId}/{reviewId}")]
        public async Task<ActionResult<FilmDto>> AddReviewAsync([FromRoute] Guid filmId, [FromRoute] Guid reviewId)
        {
            Log.Information($"AddFilmReview request received film with ID {filmId} and review with ID {reviewId}");
            await _filmService.AddReviewAsync(filmId, reviewId);
            if (reviewId != Guid.Empty)
            {
                Log.Information($"AddFilmReview response: {reviewId} added to film");
                return Ok("Review added");
            }
            Log.Error("Error adding review");
            return Ok("Error");
        }

        [HttpPut("EditFilm")]
        public async Task<ActionResult<FilmDto>> EditAsync(FilmDto filmToUpdate)
        {
            Log.Information($"EditFilm request received: {filmToUpdate}");
            FilmDto? film = await _filmService.UpdateAsync(filmToUpdate);
            if(film != null)
            {
                Log.Information($"EditFilm response: {film}");
                return Ok(film);
            }
            Log.Error($"EditFilm response: Error");
            return Ok("Error");
        }

        [HttpDelete("RemoveFilmActor/{filmId}/{actorId}")]
        public async Task<ActionResult> DeleteActorAsync([FromRoute]Guid filmId, [FromRoute]Guid actorId)
        {
            Log.Information($"RemoveFilmActor request received film with ID {filmId} and actor with ID {actorId}");
            await _filmService.RemoveActorAsync(filmId, actorId);
            Log.Information($"RemoveFilmActor response: {actorId} deleted from film");
            return Ok("Actor Removed");
        }

        [HttpDelete("RemoveFilmReview/{filmId}/{reviewId}")]
        public async Task<ActionResult> DeleteReviewAsync([FromRoute] Guid filmId, [FromRoute] Guid reviewId)
        {
            Log.Information($"RemoveFilmReview request received film with ID {filmId} and review with ID {reviewId}");
            await _filmService.RemoveReviewAsync(filmId, reviewId);
            Log.Information($"RemoveFilmReview response: {reviewId} deleted from film");
            return Ok("Review Removed");
        }

        [HttpDelete("DeleteFilm")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            Log.Information($"DeleteFilm request received ID: {id}");
            await _filmService.DeleteAsync(id);
            Log.Information($"DeleteFilm response: NoContent");
            return NoContent();
        }
    }
}
