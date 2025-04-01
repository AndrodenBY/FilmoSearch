namespace FilmoSearch.Tests.Repositories.ActorRepositoryTests.Methods;

public class GetAllAsyncTests : ActorRepositoryTestBase
{
    [Fact]
    public async Task GetAll_ReturnsAllActors()
    {
        // Arrange
        List<Actor> testActors = new List<Actor>
        {
            new Actor
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Films = new List<Film> { new Film { Id = Guid.NewGuid(), Title = "TestTitle1" } }
            },
            new Actor
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Films = new List<Film> { new Film { Id = Guid.NewGuid(), Title = "TestTitle2" } }
            }
        };

        await _context.Actors.AddRangeAsync(testActors);
        await _context.SaveChangesAsync();

        // Act
        IEnumerable<ActorDto>? result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}