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
        private ActorService _actorService;
        public ActorController(ActorService actorService) { _actorService = actorService; }

        [HttpGet("GetActors")]
        public ActionResult<IEnumerable<ActorDto>> Get()
        {
            Log.Information("Getting all actors");
            return Ok(_actorService.GetAll());
        }

        [HttpGet("GetActorById/{id}")]
        public ActionResult<ActorDto> GetById([FromRoute] Guid id)
        {
            Log.Information($"Getting actor by ID: {id}");
            ActorDto? actor = _actorService.GetById(id);
            if (actor != null)
            {
                Log.Information($"Actor found: {actor}");
                return Ok(actor);   
            }
            Log.Warning($"Actor with ID {id} not found");
            return Ok("Error");
        }

        [HttpPost("AddActor")]
        public ActionResult<ActorDto> Add(ActorDto actorToCreate)
        {
            Log.Information($"AddActor request received: {actorToCreate}");
            ActorDto? actor = _actorService.Create(actorToCreate);
            if (actor != null) 
            {
                Log.Information($"AddActor response: {actor}");
                return Ok(actor);
            }
            Log.Error("AddActor response: Error");
            return Ok("Error");
        }

        [HttpPost("AddActorFilm/{actorId}/{filmId}")]
        public ActionResult AddFilm([FromRoute] Guid actorId, [FromRoute] Guid filmId)
        {
            Log.Information($"AddActorFilm request received actor with ID {actorId} and film with ID {filmId}");
            _actorService.AddFilm(actorId, filmId);
            if (filmId != Guid.Empty)
            {
                Log.Information($"AddActorFilm response: {filmId} added to actor");
                return Ok("Film added");
            }
            Log.Error("Error adding film");
            return Ok("Error");
        }

        [HttpPut("EditActor")]
        public ActionResult<ActorDto> Edit(ActorDto actorToUpdate)
        {
            Log.Information($"EditActor request received: {actorToUpdate}");
            ActorDto? actor = _actorService.Update(actorToUpdate);
            if (actor != null)
            {
                Log.Information($"EditActor response: {actor}");
                return Ok(actor);
            }
            Log.Error("EditActor response: Error");
            return Ok("Error");
        }

        [HttpDelete("RemoveActorFilm/{actorId}/{filmId}")]
        public ActionResult DeleteFilm([FromRoute] Guid actorId, [FromRoute] Guid filmId)
        {
            Log.Information($"RemoveActorFilm request received actor with ID {actorId} and film with ID {filmId}");
            _actorService.RemoveFilm(actorId, filmId);
            Log.Information($"RemoveActorFilm response: {filmId} deleted from actor");
            return Ok("Film Removed");
        }

        [HttpDelete("DeleteActor")]
        public ActionResult Delete(Guid id)
        {
            Log.Information($"DeleteActor request received ID: {id}");
            _actorService.Delete(id);
            Log.Information("DeleteActor response: NoContent");
            return NoContent();

        }
    }
}
