using FilmoSearch.DTO;
using FilmoSearch.Repositories.Actor;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmoSearch.Services.Actor
{
    public class ActorService: IFilmoSearchService<ActorDto>
    {
        private readonly ActorRepository _repository;
        public ActorService(ActorRepository repository) { _repository = repository; }

        public IEnumerable<ActorDto> GetAll()
        {
            return _repository.GetAll();
        }

        public ActorDto GetById(Guid id)
        {
            ActorDto actor = _repository.GetById(id);
            if(actor != null)
            {
                return _repository.GetById(id);
            }
            return null;
        }

        public ActorDto Create(ActorDto actorToCreate)
        {
            bool isActorCreated = _repository.Create(actorToCreate);
            if (isActorCreated)
            {
                return actorToCreate;
            }
            return null;
        }

        public bool AddFilm(Guid actorId, Guid filmId)
        {
            bool isFilmAdded = _repository.AddFilm(actorId, filmId);
            if (isFilmAdded)
            {
                return true;
            }
            return false;
        }

        public ActorDto Update(ActorDto actorToUpdate)
        {
            bool isActorUpdated = _repository.Update(actorToUpdate);
            if (isActorUpdated)
            {
                return actorToUpdate;
            }
            return null;
        }

        public bool RemoveFilm(Guid actorId, Guid filmId)
        {
            bool isFilmDeleted = _repository.RemoveFilm(actorId, filmId);
            if (isFilmDeleted)
            {
                return true;
            }
            return false;
        }

        public bool Delete(Guid id)
        {            
            bool isActorDeleted = _repository.Delete(id);
            if (isActorDeleted)
            {
                return true;
            }
            return false;      
        }
    }
}
