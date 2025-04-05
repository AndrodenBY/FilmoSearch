using FilmoSearch.DTO;
using FilmoSearch.Repositories.Actor;

namespace FilmoSearch.Services.Actor
{
    public class ActorService: IActorService
    {
        private readonly IActorRepository _repository;
        public ActorService(IActorRepository repository) { _repository = repository; }

        public async Task<IEnumerable<ActorDto>?> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ActorDto?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ActorDto?> CreateAsync(ActorDto actorToCreate)
        {
            return await _repository.CreateAsync(actorToCreate) ? actorToCreate : null;
        }

        public async Task<bool> AddFilmAsync(Guid actorId, Guid filmId)
        {
            return await _repository.AddFilmAsync(actorId, filmId) ? true : false;
        }

        public async Task<ActorDto?> UpdateAsync(ActorDto actorToUpdate)
        {
            return await _repository.UpdateAsync(actorToUpdate) ? actorToUpdate : null;
        }

        public async Task<bool> RemoveFilmAsync(Guid actorId, Guid filmId)
        {
            return await _repository.RemoveFilmAsync(actorId, filmId) ? true : false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id) ? true : false;
        }
    }
}
