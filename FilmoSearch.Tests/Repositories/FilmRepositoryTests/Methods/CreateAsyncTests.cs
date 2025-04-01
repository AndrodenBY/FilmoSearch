namespace FilmoSearch.Tests.Repositories.FilmRepositoryTests.Methods;

public class CreateAsyncTests : FilmRepositoryTestBase
{
    [Fact]
    public async Task CreateAsync_ReturnsTrueWhenFilmIsCreatedSuccessfully()
    {
        // Arrange
        FilmDto filmToCreate = new FilmDto(
            Guid.NewGuid(),
            "TestFilm",
            new List<ReviewDto>
            {
                new ReviewDto(Guid.NewGuid(), "ReviewTitle1", "ReviewDescription1", 4, null)
            },
            new List<ActorDto>
            {
                new ActorDto(Guid.NewGuid(), "John", "Doe", null)
            }
        );

        // Act
        bool result = await _repository.CreateAsync(filmToCreate);

        // Assert
        Assert.True(result);
        Film? createdFilm = await _context.Films.Include(f => f.Reviews).Include(f => f.Actors)
            .FirstOrDefaultAsync(f => f.Id == filmToCreate.Id);
        Assert.NotNull(createdFilm);
        Assert.Equal("TestFilm", createdFilm.Title);
        Assert.Single(createdFilm.Reviews);
        Assert.Equal("ReviewTitle1", createdFilm.Reviews.First().Title);
        Assert.Single(createdFilm.Actors);
        Assert.Equal("John", createdFilm.Actors.First().FirstName);
    }

    [Fact]
    public async Task CreateAsync_ReturnsFalseWhenFilmIsNull()
    {
        // Arrange
        FilmDto? filmToCreate = null;

        // Act
        bool result = await _repository.CreateAsync(filmToCreate);

        // Assert
        Assert.False(result);
    }
}