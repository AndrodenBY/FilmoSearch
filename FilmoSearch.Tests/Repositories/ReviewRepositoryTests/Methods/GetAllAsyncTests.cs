namespace FilmoSearch.Tests.Repositories.ReviewRepositoryTests.Methods;

public class GetAllAsyncTests : ReviewRepositoryTestBase
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllReviewsSuccessfully()
    {
        // Arrange
        var testFilm = new Film
        {
            Id = Guid.NewGuid(),
            Title = "Test Film"
        };

        var testReviews = new List<Review>
        {
            new Review
            {
                Id = Guid.NewGuid(),
                Title = "Review 1",
                Description = "Description 1",
                Stars = 4,
                Film = testFilm
            },
            new Review
            {
                Id = Guid.NewGuid(),
                Title = "Review 2",
                Description = "Description 2",
                Stars = 5,
                Film = testFilm
            }
        };

        await _context.Films.AddAsync(testFilm);
        await _context.Reviews.AddRangeAsync(testReviews);
        await _context.SaveChangesAsync();

        // Act
        IEnumerable<ReviewDto>? result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Title == "Review 1");
        Assert.Contains(result, r => r.Title == "Review 2");
        ReviewDto? firstReview = result.First();
        Assert.Equal("Test Film", firstReview.Film.Title);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyListWhenNoReviewsExist()
    {
        // Act
        IEnumerable<ReviewDto>? result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

}