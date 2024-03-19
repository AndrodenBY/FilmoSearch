namespace FilmoSearch.Repositories
{
    public interface IFilmoSearchRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        bool Create(T itemToCreate);
        bool Update(T itemToUpdate);
        bool Delete(Guid id);
    }
}