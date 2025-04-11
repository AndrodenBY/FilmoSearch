using FilmoSearch.DTO;

namespace FilmoSearch.Repositories.Review;

public interface IReviewRepository : IRepository<ReviewDto>
{
    Task<bool> AddToFilmAsync(ReviewDto reviewToCreate, Guid filmId);
}