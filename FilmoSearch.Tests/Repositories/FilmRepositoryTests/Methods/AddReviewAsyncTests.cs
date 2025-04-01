namespace FilmoSearch.Tests.Repositories.FilmRepositoryTests.Methods;

public class AddReviewAsyncTests : FilmRepositoryTestBase
{
    [Fact]
    public async Task AddReviewAsync_ReturnsTrueWhenReviewIsAddedSuccessfully()
    {
        // Arrange
        var filmId = Guid.NewGuid();
        var reviewId = Guid.NewGuid();

        var testFilm = new Film
        {
            Id = filmId,
            Title = "Test Film",
            Reviews = new List<Review>()
        };

        var testReview = new Review
        {
            Id = reviewId,
            Title = "Test Review",
            Description = "Review Description",
            Stars = 5
        };

        await _context.Films.AddAsync(testFilm);
        await _context.Reviews.AddAsync(testReview);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.AddReviewAsync(filmId, reviewId);

        // Assert
        Assert.True(result);
        var updatedFilm = await _context.Films.Include(f => f.Reviews).FirstOrDefaultAsync(f => f.Id == filmId);
        Assert.NotNull(updatedFilm);
        Assert.Single(updatedFilm.Reviews);
        Assert.Equal("Test Review", updatedFilm.Reviews.First().Title);
    }

    [Fact]
    public async Task AddReviewAsync_ReturnsFalseWhenFilmDoesNotExist()
    {
        // Arrange
        var filmId = Guid.NewGuid();
        var reviewId = Guid.NewGuid();

        var testReview = new Review
        {
            Id = reviewId,
            Title = "Test Review",
            Description = "Review Description",
            Stars = 5
        };

        await _context.Reviews.AddAsync(testReview);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.AddReviewAsync(filmId, reviewId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddReviewAsync_ReturnsFalseWhenReviewDoesNotExist()
    {
        // Arrange
        var filmId = Guid.NewGuid();

        var testFilm = new Film
        {
            Id = filmId,
            Title = "Test Film",
            Reviews = new List<Review>()
        };

        await _context.Films.AddAsync(testFilm);
        await _context.SaveChangesAsync();

        var reviewId = Guid.NewGuid();

        // Act
        var result = await _repository.AddReviewAsync(filmId, reviewId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddReviewAsync_ReturnsFalseWhenReviewIsAlreadyInFilm()
    {
        // Arrange
        var filmId = Guid.NewGuid();
        var reviewId = Guid.NewGuid();

        var testReview = new Review
        {
            Id = reviewId,
            Title = "Duplicate Review",
            Description = "Duplicate Description",
            Stars = 4
        };

        var testFilm = new Film
        {
            Id = filmId,
            Title = "Test Film",
            Reviews = new List<Review> { testReview }
        };

        await _context.Films.AddAsync(testFilm);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.AddReviewAsync(filmId, reviewId);

        // Assert
        Assert.False(result);
        var updatedFilm = await _context.Films.Include(f => f.Reviews).FirstOrDefaultAsync(f => f.Id == filmId);
        Assert.NotNull(updatedFilm);
        Assert.Single(updatedFilm.Reviews);
    }
}