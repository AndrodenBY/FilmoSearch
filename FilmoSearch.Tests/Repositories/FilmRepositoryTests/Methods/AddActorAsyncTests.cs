using FilmoSearch.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmoSearch.Tests.Repositories.FilmRepositoryTests.Methods;

public class AddActorAsyncTests : FilmRepositoryTestBase
{
    [Fact]
    public async Task AddActorAsync_ReturnsTrueWhenActorIsAddedSuccessfully()
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

        Actor testActor = new Actor
        {
            Id = actorId,
            FirstName = "John",
            LastName = "Doe"
        };

        await _context.Films.AddAsync(testFilm);
        await _context.Actors.AddAsync(testActor);
        await _context.SaveChangesAsync();

        // Act
        bool result = await _repository.AddActorAsync(filmId, actorId);

        // Assert
        Assert.True(result);
        Film? updatedFilm = await _context.Films.Include(f => f.Actors).FirstOrDefaultAsync(f => f.Id == filmId);
        Assert.NotNull(updatedFilm);
        Assert.Single(updatedFilm.Actors);
        Assert.Equal("John", updatedFilm.Actors.First().FirstName);
    }

    [Fact]
    public async Task AddActorAsync_ReturnsFalseWhenFilmDoesNotExist()
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

        await _context.Actors.AddAsync(testActor);
        await _context.SaveChangesAsync();

        // Act
        bool result = await _repository.AddActorAsync(filmId, actorId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddActorAsync_ReturnsFalseWhenActorDoesNotExist()
    {
        // Arrange
        Guid filmId = Guid.NewGuid();

        Film testFilm = new Film
        {
            Id = filmId,
            Title = "Test Film",
            Actors = new List<Actor>()
        };

        await _context.Films.AddAsync(testFilm);
        await _context.SaveChangesAsync();

        Guid actorId = Guid.NewGuid();

        // Act
        bool result = await _repository.AddActorAsync(filmId, actorId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddActorAsync_ReturnsFalseWhenActorIsAlreadyInFilm()
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
        bool result = await _repository.AddActorAsync(filmId, actorId);

        // Assert
        Assert.False(result);
        Film? updatedFilm = await _context.Films.Include(f => f.Actors).FirstOrDefaultAsync(f => f.Id == filmId);
        Assert.NotNull(updatedFilm);
        Assert.Single(updatedFilm.Actors);
    }
}