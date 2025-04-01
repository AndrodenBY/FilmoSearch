namespace FilmoSearch.Tests.Repositories.ReviewRepositoryTests.Methods;

public class DeleteAsyncTests : ReviewRepositoryTestBase
{
    [Fact]
    public async Task DeleteAsync_ReturnsTrueWhenReviewIsDeletedSuccessfully()
    {
        // Arrange
        Guid reviewId = Guid.NewGuid();
        Review testReview = new Review
        {
            Id = reviewId,
            Title = "Test Review",
            Description = "Test Description",
            Stars = 5
        };

        await _context.Reviews.AddAsync(testReview);
        await _context.SaveChangesAsync();

        // Act
        bool result = await _repository.DeleteAsync(reviewId);

        // Assert
        Assert.True(result);
        Review? deletedReview = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        Assert.Null(deletedReview);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalseWhenReviewDoesNotExist()
    {
        // Arrange
        Guid nonExistentReviewId = Guid.NewGuid();

        // Act
        bool result = await _repository.DeleteAsync(nonExistentReviewId);

        // Assert
        Assert.False(result);
    }
}