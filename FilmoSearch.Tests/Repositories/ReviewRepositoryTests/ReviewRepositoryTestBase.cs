namespace FilmoSearch.Tests.Repositories.ReviewRepositoryTests;

public abstract class ReviewRepositoryTestBase
{
    protected readonly ApplicationContext _context;
    protected readonly ReviewRepository _repository;

    protected ReviewRepositoryTestBase()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationContext(options);
        _repository = new ReviewRepository(_context);
    }
}