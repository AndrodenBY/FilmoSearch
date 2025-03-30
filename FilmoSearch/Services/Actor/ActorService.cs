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
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ActorDto> CreateAsync(ActorDto actorToCreate)
        {
            return await _repository.CreateAsync(actorToCreate) ? actorToCreate : null;
        }

        public async Task<bool> AddFilmAsync(Guid actorId, Guid filmId)
        {
            return await _repository.AddFilmAsync(actorId, filmId) ? true : false;
        }

        public async Task<ActorDto> UpdateAsync(ActorDto actorToUpdate)
        {
            return await _repository.UpdateAsync(actorToUpdate) ? actorToUpdate : null;
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
            return await _repository.DeleteAsync(id) ? true : false;
        }
    }
}
