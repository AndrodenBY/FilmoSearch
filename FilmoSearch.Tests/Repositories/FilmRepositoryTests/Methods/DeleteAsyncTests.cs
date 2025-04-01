using FilmoSearch.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmoSearch.Tests.Repositories.FilmRepositoryTests.Methods;

public class DeleteAsyncTests : FilmRepositoryTestBase
{
    [Fact]
    public async Task DeleteAsync_ReturnsTrueWhenFilmIsDeletedSuccessfully()
    {
        // Arrange
        Guid filmId = Guid.NewGuid();
        Film testFilm = new Film
        {
            Id = filmId,
            Title = "Test Film"
        };

        await _context.Films.AddAsync(testFilm);
        await _context.SaveChangesAsync();

        // Act
        bool result = await _repository.DeleteAsync(filmId);

        // Assert
        Assert.True(result);
        Film? deletedFilm = await _context.Films.FirstOrDefaultAsync(f => f.Id == filmId);
        Assert.Null(deletedFilm);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalseWhenFilmDoesNotExist()
    {
        // Arrange
        Guid nonExistentFilmId = Guid.NewGuid();

        // Act
        bool result = await _repository.DeleteAsync(nonExistentFilmId);

        // Assert
        Assert.False(result);
    }
}