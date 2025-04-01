namespace FilmoSearch.Tests.Repositories.ReviewRepositoryTests.Methods;

public class GetByIdAsyncTests : ReviewRepositoryTestBase
{
    [Fact]
    public async Task GetByIdAsync_ReturnsReviewWhenIdExists()
    {
        // Arrange
        Guid reviewId = Guid.NewGuid();
        Film testFilm = new Film
        {
            Id = Guid.NewGuid(),
            Title = "Test Film"
        };

        var testReview = new Review
        {
            Id = reviewId,
            Title = "Test Review",
            Description = "Test Description",
            Stars = 4,
            Film = testFilm
        };

        await _context.Films.AddAsync(testFilm);
        await _context.Reviews.AddAsync(testReview);
        await _context.SaveChangesAsync();

        // Act
        ReviewDto? result = await _repository.GetByIdAsync(reviewId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(reviewId, result.Id);
        Assert.Equal("Test Review", result.Title);
        Assert.Equal(4, result.Stars);
        Assert.Equal("Test Film", result.Film.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenReviewDoesNotExist()
    {
        // Arrange
        Guid nonExistentReviewId = Guid.NewGuid();

        // Act
        ReviewDto? result = await _repository.GetByIdAsync(nonExistentReviewId);

        // Assert
        Assert.Null(result);
    }
}