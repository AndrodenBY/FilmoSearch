namespace FilmoSearch.Tests.Repositories.ReviewRepositoryTests.Methods;

public class AddToFilmAsyncTests : ReviewRepositoryTestBase
{
    [Fact]
    public async Task AddToFilmAsync_ReturnsTrueWhenReviewIsAddedSuccessfully()
    {
        // Arrange
        Guid filmId = Guid.NewGuid();
        ReviewDto reviewToCreate = new ReviewDto(
            Guid.NewGuid(),
            "Test Review",
            "Test Description",
            5,
            null
        );

        Film testFilm = new Film
        {
            Id = filmId,
            Title = "Test Film",
            Reviews = new List<Review>()
        };

        await _context.Films.AddAsync(testFilm);
        await _context.SaveChangesAsync();

        // Act
        bool result = await _repository.AddToFilmAsync(reviewToCreate, filmId);

        // Assert
        Assert.True(result);
        Review? createdReview = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewToCreate.Id);
        Assert.NotNull(createdReview);
        Assert.Equal("Test Review", createdReview.Title);
        Assert.Equal("Test Description", createdReview.Description);
        Assert.Equal(5, createdReview.Stars);
        Assert.Equal(filmId, createdReview.FilmId);
        Film? updatedFilm = await _context.Films.Include(f => f.Reviews).FirstOrDefaultAsync(f => f.Id == filmId);
        Assert.NotNull(updatedFilm);
        Assert.Single(updatedFilm.Reviews);
    }

    [Fact]
    public async Task AddToFilmAsync_ReturnsFalseWhenFilmDoesNotExist()
    {
        // Arrange
        Guid nonExistentFilmId = Guid.NewGuid();
        ReviewDto reviewToCreate = new ReviewDto(
            Guid.NewGuid(),
            "Test Review",
            "Test Description",
            4,
            null
        );

        // Act
        bool result = await _repository.AddToFilmAsync(reviewToCreate, nonExistentFilmId);

        // Assert
        Assert.False(result);
        Review? createdReview = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewToCreate.Id);
        Assert.Null(createdReview);
    }
}