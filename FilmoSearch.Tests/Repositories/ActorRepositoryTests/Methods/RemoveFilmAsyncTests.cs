using FilmoSearch.DTO;
using FilmoSearch.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmoSearch.Tests.Repositories.ActorRepositoryTests.Methods;

public class RemoveFilmAsyncTests : ActorRepositoryTestBase
{
    [Fact]
    public async Task RemoveFilmAsync_ReturnsTrueWhenFilmIsRemovedSuccessfully()
    {
        // Arrange
        Guid actorId = Guid.NewGuid();
        Guid filmId = Guid.NewGuid();

        Actor testActor = new Actor
        {
            Id = actorId,
            FirstName = "John",
            LastName = "Doe",
            Films = new List<Film>
            {
                new Film { Id = filmId, Title = "TestFilm1" },
                new Film { Id = Guid.NewGuid(), Title = "TestFilm2" }
            }
        };

        await _context.Actors.AddAsync(testActor);
        await _context.SaveChangesAsync();

        // Act
        bool result = await _repository.RemoveFilmAsync(actorId, filmId);

        // Assert
        Assert.True(result);
        Actor? actorFromDb = await _context.Actors.Include(a => a.Films).FirstOrDefaultAsync(a => a.Id == actorId);
        Assert.NotNull(actorFromDb);
        Assert.DoesNotContain(actorFromDb.Films, f => f.Id == filmId);
    }

    [Fact]
    public async Task RemoveFilmAsync_ReturnsFalseWhenActorDoesNotExist()
    {
        // Arrange
        Guid actorId = Guid.NewGuid();
        Guid filmId = Guid.NewGuid();

        // Act
        bool result = await _repository.RemoveFilmAsync(actorId, filmId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task RemoveFilmAsync_ReturnsTrueWhenFilmIsNotFoundInActor()
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
                new Film { Id = Guid.NewGuid(), Title = "ExistingFilm" }
            }
        };

        await _context.Actors.AddAsync(testActor);
        await _context.SaveChangesAsync();

        Guid nonExistentFilmId = Guid.NewGuid();

        // Act
        bool result = await _repository.RemoveFilmAsync(actorId, nonExistentFilmId);

        // Assert
        Assert.True(result);
        Actor? actorFromDb = await _context.Actors.Include(a => a.Films).FirstOrDefaultAsync(a => a.Id == actorId);
        Assert.NotNull(actorFromDb);
        Assert.Equal(1, actorFromDb.Films.Count);
    }

}