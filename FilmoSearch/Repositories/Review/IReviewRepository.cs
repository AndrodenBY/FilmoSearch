using FilmoSearch.DTO;

namespace FilmoSearch.Repositories.Review;

public interface IReviewRepository : IFilmoSearchRepository<ReviewDto>
{
    Task<bool> AddToFilmAsync(ReviewDto reviewToCreate, Guid filmId);
}