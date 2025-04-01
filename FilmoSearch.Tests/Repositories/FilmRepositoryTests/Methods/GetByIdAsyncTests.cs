namespace FilmoSearch.Tests.Repositories.FilmRepositoryTests.Methods;

public class GetByIdAsyncTests : FilmRepositoryTestBase
{
    [Fact]
    public async Task GetByIdAsync_ReturnsFilmWhenIdExists()
    {
        // Arrange
        Guid filmId = Guid.NewGuid();
        Film testFilm = new Film
        {
            Id = filmId,
            Title = "TestFilm",
            Reviews = new List<Review>
            {
                new Review { Id = Guid.NewGuid(), Title = "Review1", Description = "Description1", Stars = 4 }
            },
            Actors = new List<Actor>
            {
                new Actor { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" }
            }
        };

        await _context.Films.AddAsync(testFilm);
        await _context.SaveChangesAsync();

        // Act
        FilmDto? result = await _repository.GetByIdAsync(filmId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(filmId, result.Id);
        Assert.Equal("TestFilm", result.Title);
        Assert.Single(result.Reviews);
        Assert.Equal("Review1", result.Reviews.First().Title);
        Assert.Single(result.Actors);
        Assert.Equal("John", result.Actors.First().FirstName);
    }
    
    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenFilmDoesNotExist()
    {
        // Arrange
        Guid nonExistentFilmId = Guid.NewGuid();

        // Act
        FilmDto? result = await _repository.GetByIdAsync(nonExistentFilmId);

        // Assert
        Assert.Null(result);
    }
}