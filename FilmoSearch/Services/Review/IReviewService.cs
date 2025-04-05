using FilmoSearch.DTO;

namespace FilmoSearch.Services.Review;

public interface IReviewService : IFilmoSearchService<ReviewDto>
{
    Task<bool> AddToFilmAsync(ReviewDto reviewToCreate, Guid filmId);
}