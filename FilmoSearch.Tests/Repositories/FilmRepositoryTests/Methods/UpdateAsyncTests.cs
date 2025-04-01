namespace FilmoSearch.Tests.Repositories.FilmRepositoryTests.Methods;

public class UpdateAsyncTests : FilmRepositoryTestBase
{
    [Fact]
    public async Task UpdateAsync_ReturnsTrueWhenFilmIsUpdatedSuccessfully()
    {
        // Arrange
        Guid filmId = Guid.NewGuid();

        Film originalFilm = new Film
        {
            Id = filmId,
            Title = "OriginalTitle",
            Reviews = new List<Review>
            {
                new Review { Id = Guid.NewGuid(), Title = "OriginalReview", Description = "OriginalDescription", Stars = 3 }
            },
            Actors = new List<Actor>
            {
                new Actor { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" }
            }
        };

        await _context.Films.AddAsync(originalFilm);
        await _context.SaveChangesAsync();

        FilmDto filmToUpdate = new FilmDto(
            originalFilm.Id,
            "UpdatedTitle",
            new List<ReviewDto>
            {
                new (Guid.NewGuid(), "UpdatedReview", "UpdatedDescription", 5, null)
            },
            new List<ActorDto>
            {
                new (Guid.NewGuid(), "Jane", "Smith", null)
            }
        );

        // Act
        bool result = await _repository.UpdateAsync(filmToUpdate);

        // Assert
        Assert.True(result);
        Film? updatedFilm = await _context.Films.Include(f => f.Reviews).Include(f => f.Actors).FirstOrDefaultAsync(f => f.Id == filmId);
        Assert.NotNull(updatedFilm);
        Assert.Equal("UpdatedTitle", updatedFilm.Title);
        Assert.Single(updatedFilm.Reviews);
        Assert.Equal("UpdatedReview", updatedFilm.Reviews.First().Title);
        Assert.Single(updatedFilm.Actors);
        Assert.Equal("Jane", updatedFilm.Actors.First().FirstName);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalseWhenFilmDoesNotExist()
    {
        // Arrange
        FilmDto filmToUpdate = new FilmDto(
            Guid.NewGuid(),
            "NonexistentFilmTitle",
            null,
            null
        );

        // Act
        bool result = await _repository.UpdateAsync(filmToUpdate);

        // Assert
        Assert.False(result);
    }
}