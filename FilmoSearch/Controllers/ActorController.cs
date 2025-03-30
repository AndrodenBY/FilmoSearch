using FilmoSearch.DTO;
using FilmoSearch.Models;
using FilmoSearch.Services.Actor;
using FilmoSearch.Services.Film;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace FilmoSearch.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly ActorService _actorService;
        public ActorController(ActorService actorService) { _actorService = actorService; }

        [HttpGet("GetActors")]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetAsync()
        {
            Log.Information("Getting all actors");
            return Ok(await _actorService.GetAllAsync());
        }

        [HttpGet("GetActorById/{id}")]
        public async Task<ActionResult<ActorDto>> GetByIdAsync([FromRoute] Guid id)
        {
            Log.Information($"Getting actor by ID: {id}");
            ActorDto? actor = await _actorService.GetByIdAsync(id);
            if (actor != null)
            {
                Log.Information($"Actor found: {actor}"); // add to construction/ DI
                return Ok(actor);   
            }
            Log.Warning($"Actor with ID {id} not found");
            return Ok("Error"); //return json
        }

        [HttpPost("AddActor")]
        public async Task<ActionResult<ActorDto>> AddAsync(ActorDto actorToCreate)
        {
            Log.Information($"AddActor request received: {actorToCreate}");
            ActorDto? actor = await _actorService.CreateAsync(actorToCreate);
            if (actor != null) 
            {
                Log.Information($"AddActor response: {actor}");
                return Ok(actor);
            }
            Log.Error("AddActor response: Error");
            return Ok("Error");
        }

        [HttpPost("AddActorFilm/{actorId}/{filmId}")]
        public async Task<ActionResult> AddFilmAsync([FromRoute] Guid actorId, [FromRoute] Guid filmId)
        {
            Log.Information($"AddActorFilm request received actor with ID {actorId} and film with ID {filmId}");
            await _actorService.AddFilmAsync(actorId, filmId);
            if (filmId != Guid.Empty)
            {
                Log.Information($"AddActorFilm response: {filmId} added to actor");
                return Ok("Film added");
            }
            Log.Error("Error adding film");
            return Ok("Error");
        }

        [HttpPut("EditActor")]
        public async Task<ActionResult<ActorDto>> EditAsync(ActorDto actorToUpdate)
        {
            Log.Information($"EditActor request received: {actorToUpdate}");
            ActorDto? actor = await _actorService.UpdateAsync(actorToUpdate);
            if (actor != null)
            {
                Log.Information($"EditActor response: {actor}");
                return Ok(actor);
            }
            Log.Error("EditActor response: Error");
            return Ok("Error");
        }

        [HttpDelete("RemoveActorFilm/{actorId}/{filmId}")]
        public async Task<ActionResult> DeleteFilmAsync([FromRoute] Guid actorId, [FromRoute] Guid filmId)
        {
            Log.Information($"RemoveActorFilm request received actor with ID {actorId} and film with ID {filmId}");
            await _actorService.RemoveFilmAsync(actorId, filmId);
            Log.Information($"RemoveActorFilm response: {filmId} deleted from actor");
            return Ok("Film Removed");
        }

        [HttpDelete("DeleteActor")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            Log.Information($"DeleteActor request received ID: {id}");
            await _actorService.DeleteAsync(id);
            Log.Information("DeleteActor response: NoContent");
            return NoContent();
        }
    }
}
