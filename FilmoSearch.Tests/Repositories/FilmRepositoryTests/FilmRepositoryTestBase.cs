namespace FilmoSearch.Tests.Repositories.FilmRepositoryTests;

public class FilmRepositoryTestBase
{
        protected readonly ApplicationContext _context;
        protected readonly FilmRepository _repository;

        protected FilmRepositoryTestBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationContext(options);
            _repository = new FilmRepository(_context);
        }
}