using FilmoSearch.DTO;
using FilmoSearch.Models;

namespace FilmoSearch.Tests.Repositories.ActorRepositoryTests.Methods;

public class GetByIdAsyncTests : ActorRepositoryTestBase
{
    [Fact]
    public async Task GetByIdAsync_ReturnsActorById()
    {
        // Arrange
        Guid actorId = Guid.NewGuid();
        Actor testActor = new Actor
        {
            Id = actorId,
            FirstName = "John",
            LastName = "Doe",
            Films = new List<Film>
            {
                new Film { Id = Guid.NewGuid(), Title = "TestFilm1" },
                new Film { Id = Guid.NewGuid(), Title = "TestFilm2" }
            }
        };

        await _context.Actors.AddAsync(testActor);
        await _context.SaveChangesAsync();

        // Act
        ActorDto? result = await _repository.GetByIdAsync(actorId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(actorId, result.Id);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName); 
        Assert.Equal(2, result.Films.Count); 
        Assert.Contains(result.Films, f => f.Title == "TestFilm1"); 
        Assert.Contains(result.Films, f => f.Title == "TestFilm2");
    }
}