namespace FilmoSearch.Services
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>?> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> CreateAsync(T itemToCreate);
        Task<T?> UpdateAsync(T itemToUpdate);
        Task<bool> DeleteAsync(Guid id);
    }
}