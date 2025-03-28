namespace FilmoSearch.Repositories
{
    public interface IFilmoSearchRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(T itemToCreate);
        Task<bool> UpdateAsync(T itemToUpdate);
        Task<bool> DeleteAsync(Guid id);
    }
}