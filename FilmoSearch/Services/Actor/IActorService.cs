using FilmoSearch.DTO;

namespace FilmoSearch.Services.Actor;

public interface IActorService : IService<ActorDto>
{
    Task<bool> RemoveFilmAsync(Guid actorId, Guid filmId);
    Task<bool> AddFilmAsync(Guid actorId, Guid filmId);
}