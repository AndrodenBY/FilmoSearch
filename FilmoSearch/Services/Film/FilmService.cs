using FilmoSearch.DTO;
using FilmoSearch.Repositories.Film;

namespace FilmoSearch.Services.Film
{
    public class FilmService : IFilmoSearchService<FilmDto>
    {
        private readonly FilmRepository _repository;
        public FilmService(FilmRepository repository) { _repository = repository; }

        public async Task<IEnumerable<FilmDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<FilmDto> GetByIdAsync(Guid id)
        {
            FilmDto film = await _repository.GetByIdAsync(id);
            if (film != null)
            {
                return await _repository.GetByIdAsync(id);
            }
            return null;
        }

        public async Task<FilmDto> CreateAsync(FilmDto filmToCreate)
        {
            bool isFilmCreated = await _repository.CreateAsync(filmToCreate);
            if (isFilmCreated)
            {
                return filmToCreate;
            }
            return null;
        }

        public async Task<bool> AddActorAsync(Guid filmId, Guid actorId)
        {
            bool isActorAdded = await _repository.AddActorAsync(filmId, actorId);
            if (isActorAdded)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AddReviewAsync(Guid filmId, Guid reviewId)
        {
            bool isReviewAdded = await _repository.AddReviewAsync(filmId, reviewId);
            if (isReviewAdded)
            {
                return true;
            }
            return false;
        }

        public async Task<FilmDto> UpdateAsync(FilmDto filmToUpdate)
        {
            bool isFilmUpdated = await _repository.UpdateAsync(filmToUpdate);
            if (isFilmUpdated)
            {
                return filmToUpdate;
            }
            return null;
        }

        public async Task<bool> RemoveActorAsync(Guid filmId, Guid actorId) 
        {
            bool isActorDeleted = await _repository.RemoveActorAsync(filmId, actorId);
            if(isActorDeleted)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveReviewAsync(Guid filmId, Guid reviewId)
        {
            bool isReviewDeleted = await _repository.RemoveReviewAsync(filmId, reviewId);
            if (isReviewDeleted)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool isFilmDeleted = await _repository.DeleteAsync(id);
            if (isFilmDeleted)
            {
                return true;
            }
            return false;
        }
    }
}
