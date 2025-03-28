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

        public async Task<IEnumerable<ActorDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ActorDto> GetByIdAsync(Guid id)
        {
            ActorDto actor = await _repository.GetByIdAsync(id);
            if(actor != null)
            {
                return await _repository.GetByIdAsync(id);
            }
            return null;
        }

        public async Task<ActorDto> CreateAsync(ActorDto actorToCreate)
        {
            bool isActorCreated = await _repository.CreateAsync(actorToCreate);
            if (isActorCreated)
            {
                return actorToCreate;
            }
            return null;
        }

        public async Task<bool> AddFilmAsync(Guid actorId, Guid filmId)
        {
            bool isFilmAdded = await _repository.AddFilmAsync(actorId, filmId);
            if (isFilmAdded)
            {
                return true;
            }
            return false;
        }

        public async Task<ActorDto> UpdateAsync(ActorDto actorToUpdate)
        {
            bool isActorUpdated = await _repository.UpdateAsync(actorToUpdate);
            if (isActorUpdated)
            {
                return actorToUpdate;
            }
            return null;
        }

        public async Task<bool> RemoveFilmAsync(Guid actorId, Guid filmId)
        {
            bool isFilmDeleted = await _repository.RemoveFilmAsync(actorId, filmId);
            if (isFilmDeleted)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {            
            bool isActorDeleted = await _repository.DeleteAsync(id);
            if (isActorDeleted)
            {
                return true;
            }
            return false;      
        }
    }
}
