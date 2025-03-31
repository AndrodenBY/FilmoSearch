using FilmoSearch.DTO;

namespace FilmoSearch.Tests.Repositories.ActorRepositoryTests.Methods;

public class CreateAsyncTests : ActorRepositoryTestBase
{
    [Fact]
    public async Task CreateAsync_ReturnsTrueWhenActorIsCreated()
    {
        // Arrange
        ActorDto actorToCreate = new ActorDto
        (
            Id: Guid.NewGuid(),
            FirstName: "John",
            LastName: "Doe",
            Films: new List<FilmDto> 
            {
                new (Guid.NewGuid(), "TestFilm1", null, null),
                new (Guid.NewGuid(), "TestFilm2", null, null)
            }
        );

        // Act
        bool result = await _repository.CreateAsync(actorToCreate);

        // Assert
        Assert.True(result); 
        var createdActor = await _context.Actors.FindAsync(actorToCreate.Id);
        Assert.NotNull(createdActor);
        Assert.Equal(actorToCreate.FirstName, createdActor.FirstName);
        Assert.Equal(actorToCreate.LastName, createdActor.LastName); 
        Assert.Equal(actorToCreate.Films.Count, createdActor.Films.Count); 
        Assert.Contains(createdActor.Films, f => f.Title == "TestFilm1"); 
        Assert.Contains(createdActor.Films, f => f.Title == "TestFilm2"); 
    }
}