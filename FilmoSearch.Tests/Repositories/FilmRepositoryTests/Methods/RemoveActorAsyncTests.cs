namespace FilmoSearch.Tests.Repositories.FilmRepositoryTests.Methods;

public class RemoveActorAsyncTests : FilmRepositoryTestBase
{
    [Fact]
    public async Task RemoveActorAsync_ReturnsTrueWhenActorIsRemovedSuccessfully()
    {
        // Arrange
        Guid filmId = Guid.NewGuid();
        Guid actorId = Guid.NewGuid();

        Actor testActor = new Actor
        {
            Id = actorId,
            FirstName = "John",
            LastName = "Doe"
        };

        Film testFilm = new Film
        {
            Id = filmId,
            Title = "Test Film",
            Actors = new List<Actor> { testActor }
        };

        await _context.Films.AddAsync(testFilm);
        await _context.SaveChangesAsync();

        // Act
        bool result = await _repository.RemoveActorAsync(filmId, actorId);

        // Assert
        Assert.True(result);
        Film? updatedFilm = await _context.Films.Include(f => f.Actors).FirstOrDefaultAsync(f => f.Id == filmId);
        Assert.NotNull(updatedFilm);
        Assert.Empty(updatedFilm.Actors);
    }

    [Fact]
    public async Task RemoveActorAsync_ReturnsFalseWhenFilmDoesNotExist()
    {
        // Arrange
        Guid filmId = Guid.NewGuid();
        Guid actorId = Guid.NewGuid();

        // Act
        bool result = await _repository.RemoveActorAsync(filmId, actorId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task RemoveActorAsync_ReturnsTrueWhenActorDoesNotExistInFilm()
    {
        // Arrange
        Guid filmId = Guid.NewGuid();
        Guid actorId = Guid.NewGuid();

        Film testFilm = new Film
        {
            Id = filmId,
            Title = "Test Film",
            Actors = new List<Actor>()
        };

        await _context.Films.AddAsync(testFilm);
        await _context.SaveChangesAsync();

        // Act
        bool result = await _repository.RemoveActorAsync(filmId, actorId);

        // Assert
        Assert.True(result);
        Film? updatedFilm = await _context.Films.Include(f => f.Actors).FirstOrDefaultAsync(f => f.Id == filmId);
        Assert.NotNull(updatedFilm);
        Assert.Empty(updatedFilm.Actors);
    }
}