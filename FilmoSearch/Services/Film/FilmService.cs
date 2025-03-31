using FilmoSearch.DTO;
using FilmoSearch.Repositories.Film;

namespace FilmoSearch.Services.Film
{
    public class FilmService : IFilmoSearchService<FilmDto>
    {
        private readonly FilmRepository _repository;
        public FilmService(FilmRepository repository) { _repository = repository; }

        public async Task<IEnumerable<FilmDto>?> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<FilmDto?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id); 
        }

        public async Task<FilmDto?> CreateAsync(FilmDto filmToCreate)
        {
            return await _repository.CreateAsync(filmToCreate) ? filmToCreate : null;
        }

        public async Task<bool> AddActorAsync(Guid filmId, Guid actorId)
        {
            return await _repository.AddActorAsync(filmId, actorId) ? true : false;
        }

        public async Task<bool> AddReviewAsync(Guid filmId, Guid reviewId)
        {
            return await _repository.AddReviewAsync(filmId, reviewId) ? true : false;
        }

        public async Task<FilmDto?> UpdateAsync(FilmDto filmToUpdate)
        {
            return await _repository.UpdateAsync(filmToUpdate) ? filmToUpdate : null;
        }

        public async Task<bool> RemoveActorAsync(Guid filmId, Guid actorId)
        {
            return await _repository.RemoveActorAsync(filmId, actorId) ? true : false;
        }

        public async Task<bool> RemoveReviewAsync(Guid filmId, Guid reviewId)
        {
            return await _repository.RemoveReviewAsync(filmId, reviewId) ? true : false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id) ? true : false;
        }
    }
}
