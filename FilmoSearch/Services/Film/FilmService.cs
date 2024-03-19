using FilmoSearch.DTO;
using FilmoSearch.Repositories.Film;

namespace FilmoSearch.Services.Film
{
    public class FilmService : IFilmoSearchService<FilmDto>
    {
        private readonly FilmRepository _repository;
        public FilmService(FilmRepository repository) { _repository = repository; }

        public IEnumerable<FilmDto> GetAll()
        {
            return _repository.GetAll();
        }

        public FilmDto GetById(Guid id)
        {
            FilmDto film = _repository.GetById(id);
            if (film != null)
            {
                return _repository.GetById(id);
            }
            return null;
        }

        public FilmDto Create(FilmDto filmToCreate)
        {
            bool isFilmCreated = _repository.Create(filmToCreate);
            if (isFilmCreated)
            {
                return filmToCreate;
            }
            return null;
        }

        public bool AddActor(Guid filmId, Guid actorId)
        {
            bool isActorAdded = _repository.AddActor(filmId, actorId);
            if (isActorAdded)
            {
                return true;
            }
            return false;
        }

        public bool AddReview(Guid filmId, Guid reviewId)
        {
            bool isReviewAdded = _repository.AddReview(filmId, reviewId);
            if (isReviewAdded)
            {
                return true;
            }
            return false;
        }

        public FilmDto Update(FilmDto filmToUpdate)
        {
            bool isFilmUpdated = _repository.Update(filmToUpdate);
            if (isFilmUpdated)
            {
                return filmToUpdate;
            }
            return null;
        }

        public bool RemoveActor(Guid filmId, Guid actorId) 
        {
            bool isActorDeleted = _repository.RemoveActor(filmId, actorId);
            if(isActorDeleted)
            {
                return true;
            }
            return false;
        }

        public bool RemoveReview(Guid filmId, Guid reviewId)
        {
            bool isReviewDeleted = _repository.RemoveReview(filmId, reviewId);
            if (isReviewDeleted)
            {
                return true;
            }
            return false;
        }

        public bool Delete(Guid id)
        {
            bool isFilmDeleted = _repository.Delete(id);
            if (isFilmDeleted)
            {
                return true;
            }
            return false;
        }
    }
}
