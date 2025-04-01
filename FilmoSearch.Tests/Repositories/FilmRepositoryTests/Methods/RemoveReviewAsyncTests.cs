using FilmoSearch.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmoSearch.Tests.Repositories.FilmRepositoryTests.Methods;

public class RemoveReviewAsyncTests : FilmRepositoryTestBase
{
    [Fact]
    public async Task RemoveReviewAsync_ReturnsTrueWhenReviewIsRemovedSuccessfully()
    {
        // Arrange
        Guid filmId = Guid.NewGuid();
        Guid reviewId = Guid.NewGuid();

        Review testReview = new Review
        {
            Id = reviewId,
            Title = "Test Review",
            Description = "Test Description",
            Stars = 5
        };

        Film testFilm = new Film
        {
            Id = filmId,
            Title = "Test Film",
            Reviews = new List<Review> { testReview }
        };

        await _context.Films.AddAsync(testFilm);
        await _context.SaveChangesAsync();

        // Act
        bool result = await _repository.RemoveReviewAsync(filmId, reviewId);

        // Assert
        Assert.True(result); // Ensure the method returns true
        Film? updatedFilm = await _context.Films.Include(f => f.Reviews).FirstOrDefaultAsync(f => f.Id == filmId);
        Assert.NotNull(updatedFilm);
        Assert.Empty(updatedFilm.Reviews); // Ensure the review was removed
    }

    [Fact]
    public async Task RemoveReviewAsync_ReturnsFalseWhenFilmDoesNotExist()
    {
        // Arrange
        Guid filmId = Guid.NewGuid(); // Film does not exist
        Guid reviewId = Guid.NewGuid();

        // Act
        bool result = await _repository.RemoveReviewAsync(filmId, reviewId);

        // Assert
        Assert.False(result); // Ensure the method returns false
    }

    [Fact]
    public async Task RemoveReviewAsync_ReturnsTrueWhenReviewDoesNotExistInFilm()
    {
        // Arrange
        Guid filmId = Guid.NewGuid();
        Guid reviewId = Guid.NewGuid(); // Review does not exist in the film

        Film testFilm = new Film
        {
            Id = filmId,
            Title = "Test Film",
            Reviews = new List<Review>()
        };

        await _context.Films.AddAsync(testFilm);
        await _context.SaveChangesAsync();

        // Act
        bool result = await _repository.RemoveReviewAsync(filmId, reviewId);

        // Assert
        Assert.True(result); // Ensure the method returns true
        Film? updatedFilm = await _context.Films.Include(f => f.Reviews).FirstOrDefaultAsync(f => f.Id == filmId);
        Assert.NotNull(updatedFilm);
        Assert.Empty(updatedFilm.Reviews); // Ensure no reviews were removed since none existed
    }
}