namespace FilmoSearch.Tests.Repositories.ReviewRepositoryTests.Methods;

public class UpdateAsyncTests : ReviewRepositoryTestBase
{
    [Fact]
    public async Task UpdateAsync_ReturnsTrueWhenReviewIsUpdatedSuccessfully()
    {
        // Arrange
        Guid reviewId = Guid.NewGuid();

        Review existingReview = new Review
        {
            Id = reviewId,
            Title = "Old Title",
            Description = "Old Description",
            Stars = 3
        };

        await _context.Reviews.AddAsync(existingReview);
        await _context.SaveChangesAsync();

        ReviewDto reviewToUpdate = new ReviewDto(
            reviewId,
            "New Title",
            "New Description",
            5,
            null
        );

        // Act
        bool result = await _repository.UpdateAsync(reviewToUpdate);

        // Assert
        Assert.True(result);
        Review? updatedReview = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        Assert.NotNull(updatedReview);
        Assert.Equal("New Title", updatedReview.Title);
        Assert.Equal("New Description", updatedReview.Description);
        Assert.Equal(5, updatedReview.Stars);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalseWhenReviewDoesNotExist()
    {
        // Arrange
        Guid nonExistentReviewId = Guid.NewGuid();
        ReviewDto reviewToUpdate = new ReviewDto(
            nonExistentReviewId,
            "New Title",
            "New Description",
            5,
            null
        );

        // Act
        bool result = await _repository.UpdateAsync(reviewToUpdate);

        // Assert
        Assert.False(result);
    }
}