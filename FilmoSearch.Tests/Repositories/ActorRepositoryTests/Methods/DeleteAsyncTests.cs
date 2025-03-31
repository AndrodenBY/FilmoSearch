using FilmoSearch.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmoSearch.Tests.Repositories.ActorRepositoryTests.Methods;

public class DeleteAsyncTests : ActorRepositoryTestBase
{
    
    [Fact]
    public async Task DeleteAsync_ReturnsTrueWhenActorIsDeletedSuccessfully()
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

        // Act
        bool result = await _repository.DeleteAsync(actorId);

        // Assert
        Assert.True(result);
        Actor? actorFromDb = await _context.Actors.FirstOrDefaultAsync(a => a.Id == actorId);
        Assert.Null(actorFromDb);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalseWhenActorDoesNotExist()
    {
        // Arrange
        Guid nonExistentActorId = Guid.NewGuid();

        // Act
        bool result = await _repository.DeleteAsync(nonExistentActorId);

        // Assert
        Assert.False(result);
    }
}