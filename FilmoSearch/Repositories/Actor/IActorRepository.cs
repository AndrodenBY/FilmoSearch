using FilmoSearch.DTO;

namespace FilmoSearch.Repositories.Actor;

public interface IActorRepository : IFilmoSearchRepository<ActorDto>
{
    Task<bool> AddFilmAsync(Guid actorId, Guid filmId);
    Task<bool> RemoveFilmAsync(Guid actorId, Guid filmId);
}