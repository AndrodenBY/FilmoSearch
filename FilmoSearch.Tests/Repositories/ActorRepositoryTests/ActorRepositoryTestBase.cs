namespace FilmoSearch.Tests.Repositories.ActorRepositoryTests;

public abstract class ActorRepositoryTestBase
{
    protected readonly ApplicationContext _context;
    protected readonly ActorRepository _repository;

    protected ActorRepositoryTestBase()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationContext(options);
        _repository = new ActorRepository(_context);
    }
}
