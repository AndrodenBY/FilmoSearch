namespace FilmoSearch.Services
{
    public interface IFilmoSearchService<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        T Create(T itemToCreate);
        T Update(T itemToUpdate);
        bool Delete(Guid id);
    }
}