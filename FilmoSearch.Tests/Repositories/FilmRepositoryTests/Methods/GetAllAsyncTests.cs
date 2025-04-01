namespace FilmoSearch.Tests.Repositories.FilmRepositoryTests.Methods;

public class GetAllAsyncTests : FilmRepositoryTestBase
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllFilms()
    {
        // Arrange
        List<Film> testFilms = new List<Film>
        {
            new Film
            {
                Id = Guid.NewGuid(),
                Title = "FilmTitle1",
                Reviews = new List<Review>
                {
                    new Review { Id = Guid.NewGuid(), Title = "Review1", Description = "Description1", Stars = 4 }
                },
                Actors = new List<Actor>
                {
                    new Actor { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" }
                }
            },
            new Film
            {
                Id = Guid.NewGuid(),
                Title = "FilmTitle2",
                Reviews = new List<Review>
                {
                    new Review { Id = Guid.NewGuid(), Title = "Review2", Description = "Description2", Stars = 5 }
                },
                Actors = new List<Actor>
                {
                    new Actor { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
                }
            }
        };

        await _context.Films.AddRangeAsync(testFilms);
        await _context.SaveChangesAsync();

        // Act
        IEnumerable<FilmDto>? result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result); 
        Assert.Equal(2, result.Count());

        FilmDto? firstFilm = result.First();
        Assert.Equal("FilmTitle1", firstFilm.Title);
        Assert.Single(firstFilm.Reviews);
        Assert.Equal("Review1", firstFilm.Reviews.First().Title);
        Assert.Single(firstFilm.Actors);
        Assert.Equal("John", firstFilm.Actors.First().FirstName);
    }
}