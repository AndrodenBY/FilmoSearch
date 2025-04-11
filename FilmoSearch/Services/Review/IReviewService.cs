using FilmoSearch.DTO;

namespace FilmoSearch.Services.Review;

public interface IReviewService : IService<ReviewDto>
{
    Task<bool> AddToFilmAsync(ReviewDto reviewToCreate, Guid filmId);
}