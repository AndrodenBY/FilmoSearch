namespace FilmoSearch.Tests.Repositories.ReviewRepositoryTests.Methods;

public class CreateAsyncTests : ReviewRepositoryTestBase
{
    [Fact]
    public async Task CreateAsync_ReturnsTrueWhenReviewIsCreatedSuccessfully()
    {
        // Arrange
        ReviewDto reviewToCreate = new ReviewDto(
            Guid.NewGuid(),
            "Test Review",
            "Test Description",
            4,
            null
        );

        // Act
        bool result = await _repository.CreateAsync(reviewToCreate);

        // Assert
        Assert.True(result);
        Review? createdReview = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewToCreate.Id);
        Assert.NotNull(createdReview); 
        Assert.Equal("Test Review", createdReview.Title);
        Assert.Equal("Test Description", createdReview.Description);
        Assert.Equal(4, createdReview.Stars);
    }

    [Fact]
    public async Task CreateAsync_ReturnsFalseWhenReviewToCreateIsNull()
    {
        // Arrange
        ReviewDto? reviewToCreate = null;

        // Act
        bool result = await _repository.CreateAsync(reviewToCreate);

        // Assert
        Assert.False(result);
    }
}