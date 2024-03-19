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
        private FilmService _filmService;        
        public FilmController(FilmService filmService) { _filmService = filmService; }

        [HttpGet("GetFilms")]
        public ActionResult<IEnumerable<Film>> Get()
        {
            Log.Information("Getting all films");
            return Ok(_filmService.GetAll());
        }

        [HttpGet("GetFilmById/{id}")]
        public ActionResult<FilmDto> GetById([FromRoute] Guid id)
        {
            Log.Information($"Getting film by ID: {id}");
            FilmDto? film = _filmService.GetById(id);
            if (film != null)
            {
                Log.Information($"Film found: {film}");
                return Ok(film);
            }
            Log.Warning($"Film with ID {id} not found");
            return Ok("Error"); 
        }        

        [HttpPost("AddFilm")]
        public ActionResult<FilmDto> Add(FilmDto filmToCreate)
        {
            Log.Information($"AddFilm request recieved: {filmToCreate}");
            FilmDto? film = _filmService.Create(filmToCreate);
            if (film != null)
            {
                Log.Information($"AddFilm response: {film}");
                return Ok(film);
            }
            Log.Error("AddFilm responce: Error");
            return Ok("Error");
        }

        [HttpPost("AddFilmActor/{filmId}/{actorId}")]
        public ActionResult<FilmDto> AddActor([FromRoute] Guid filmId, [FromRoute] Guid actorId)
        {
            Log.Information($"AddFilmActor request received film with ID {filmId} and actor with ID {actorId}");
            _filmService.AddActor(filmId, actorId);
            if (actorId != Guid.Empty)
            {
                Log.Information($"AddFilmActor response: {actorId} added to film");
                return Ok("Actor added");
            }
            Log.Error("Error adding actor");
            return Ok("Error");
        }
        
        [HttpPost("AddFilmReview/{filmId}/{reviewId}")]
        public ActionResult<FilmDto> AddReview([FromRoute] Guid filmId, [FromRoute] Guid reviewId)
        {
            Log.Information($"AddFilmReview request received film with ID {filmId} and review with ID {reviewId}");
            _filmService.AddReview(filmId, reviewId);
            if (reviewId != Guid.Empty)
            {
                Log.Information($"AddFilmReview response: {reviewId} added to film");
                return Ok("Review added");
            }
            Log.Error("Error adding review");
            return Ok("Error");
        }

        [HttpPut("EditFilm")]
        public ActionResult<FilmDto> Edit(FilmDto filmToUpdate)
        {
            Log.Information($"EditFilm request received: {filmToUpdate}");
            FilmDto? film = _filmService.Update(filmToUpdate);
            if(film != null)
            {
                Log.Information($"EditFilm response: {film}");
                return Ok(film);
            }
            Log.Error($"EditFilm response: Error");
            return Ok("Error");
        }

        [HttpDelete("RemoveFilmActor/{filmId}/{actorId}")]
        public ActionResult DeleteActor([FromRoute]Guid filmId, [FromRoute]Guid actorId)
        {
            Log.Information($"RemoveFilmActor request received film with ID {filmId} and actor with ID {actorId}");
            _filmService.RemoveActor(filmId, actorId);
            Log.Information($"RemoveFilmActor response: {actorId} deleted from film");
            return Ok("Actor Removed");
        }

        [HttpDelete("RemoveFilmReview/{filmId}/{reviewId}")]
        public ActionResult DeleteReview([FromRoute] Guid filmId, [FromRoute] Guid reviewId)
        {
            Log.Information($"RemoveFilmReview request received film with ID {filmId} and review with ID {reviewId}");
            _filmService.RemoveActor(filmId, reviewId);
            Log.Information($"RemoveFilmReview response: {reviewId} deleted from film");
            return Ok("Review Removed");
        }

        [HttpDelete("DeleteFilm")]
        public ActionResult Delete(Guid id)
        {
            Log.Information($"DeleteFilm request received ID: {id}");
            _filmService.Delete(id);
            Log.Information($"DeleteFilm response: NoContent");
            return NoContent();
        }
    }
}
