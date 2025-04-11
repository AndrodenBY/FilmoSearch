using FilmoSearch.DTO;

namespace FilmoSearch.Services.Film;

public interface IFilmService : IService<FilmDto>
{
    Task<bool> AddActorAsync(Guid filmId, Guid actorId);
    Task<bool> AddReviewAsync(Guid filmId, Guid reviewId);
    Task<bool> RemoveActorAsync(Guid filmId, Guid actorId);
    Task<bool> RemoveReviewAsync(Guid filmId, Guid reviewId);
}