namespace FilmoSearch.Tests.Repositories.ActorRepositoryTests.Methods;

public class UpdateAsyncTests : ActorRepositoryTestBase
{
    
    [Fact]
    public async Task UpdateAsync_ReturnsTrueWhenActorIsUpdatedSuccessfully()
    {
        // Arrange
        Guid actorId = Guid.NewGuid();
        Actor originalActor = new Actor
        {
            Id = actorId,
            FirstName = "OriginalFirstName",
            LastName = "OriginalLastName",
            Films = new List<Film>
            {
                new Film { Id = Guid.NewGuid(), Title = "OriginalFilm1" },
            }
        };

        await _context.Actors.AddAsync(originalActor);
        await _context.SaveChangesAsync();

        ActorDto actorToUpdate = new ActorDto(
            Id: actorId,
            FirstName: "UpdatedFirstName",
            LastName: "UpdatedLastName",
            Films: new List<FilmDto>
            {
                new FilmDto(Guid.NewGuid(), "UpdatedFilm1", null, null),
            }
        );

        // Act
        bool result = await _repository.UpdateAsync(actorToUpdate);

        // Assert
        Assert.True(result);
        Actor? updatedActor = await _context.Actors.Include(a => a.Films).FirstOrDefaultAsync(a => a.Id == actorId);
        Assert.NotNull(updatedActor); 
        Assert.Equal(actorToUpdate.FirstName, updatedActor.FirstName);
        Assert.Equal(actorToUpdate.LastName, updatedActor.LastName);
        Assert.Single(updatedActor.Films); 
        Assert.Equal("UpdatedFilm1", updatedActor.Films.First().Title);
    }
    
    [Fact]
    public async Task UpdateAsync_ReturnsFalseWhenActorDoesNotExist()
    {
        // Arrange
        ActorDto actorToUpdate = new ActorDto(
            Id: Guid.NewGuid(),
            FirstName: "NonExistentFirstName",
            LastName: "NonExistentLastName",
            Films: new List<FilmDto>
            {
                new FilmDto(Guid.NewGuid(), "NonExistentFilm", null, null),
            }
        );

        // Act
        bool result = await _repository.UpdateAsync(actorToUpdate);

        // Assert
        Assert.False(result);
    }

}