namespace FilmoSearch.Tests.Repositories.ActorRepositoryTests.Methods;

public class AddFilmAsyncTests : ActorRepositoryTestBase
{
    [Fact]
    public async Task AddFilmAsync_ReturnsFalseWhenActorDoesNotExist()
    {
        // Arrange
        Guid actorId = Guid.NewGuid();
        Guid filmId = Guid.NewGuid();

        // Act
        bool result = await _repository.AddFilmAsync(actorId, filmId);

        // Assert
        Assert.False(result);
    }
        
    [Fact]
    public async Task AddFilmAsync_ReturnsFalseWhenFilmDoesNotExist()
    {
        // Arrange
        Guid actorId = Guid.NewGuid();
        Actor testActor = new Actor
        {
            Id = actorId,
            FirstName = "John",
            LastName = "Doe",
            Films = new List<Film>()
        };

        await _context.Actors.AddAsync(testActor);
        await _context.SaveChangesAsync();

        Guid filmId = Guid.NewGuid();

        // Act
        bool result = await _repository.AddFilmAsync(actorId, filmId);

        // Assert
        Assert.False(result);
    }
}