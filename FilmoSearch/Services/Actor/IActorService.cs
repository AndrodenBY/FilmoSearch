using FilmoSearch.DTO;

namespace FilmoSearch.Services.Actor;

public interface IActorService : IFilmoSearchService<ActorDto>
{
    Task<bool> RemoveFilmAsync(Guid actorId, Guid filmId);
    Task<bool> AddFilmAsync(Guid actorId, Guid filmId);
}