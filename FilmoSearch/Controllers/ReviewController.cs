using FilmoSearch.DTO;
using FilmoSearch.Services.Review;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FilmoSearch.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;
        public ReviewController(ReviewService reviewService) { _reviewService = reviewService; }

        [HttpGet("GetReviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAsync()
        {
            Log.Information("Getting all reviews");
            return Ok(await _reviewService.GetAllAsync());
        }

        [HttpGet("GetReviewById/{id}")]
        public async Task<ActionResult<ReviewDto>> GetByIdAsync([FromRoute]Guid id)
        {
            Log.Information($"Getting film by ID {id}");
            ReviewDto? review = await _reviewService.GetByIdAsync(id);
            if (review != null)
            {
                Log.Information($"Review found: {review}");
                return Ok(review);
            }
            Log.Warning($"Review with ID {id} not found");
            return Ok("Error");
        }

        [HttpPost("AddReview")]
        public async Task<ActionResult<ReviewDto>> AddAsync(ReviewDto reviewToCreate)
        {
            Log.Information($"AddReview request received: {reviewToCreate}");
            ReviewDto? review = await _reviewService.CreateAsync(reviewToCreate);
            if (review != null)
            {
                Log.Information($"AddReview response: {review}");
                return Ok(review);
            }
            Log.Error("AddReview responce: Error");
            return Ok("Error");
        }

        [HttpPost("AddReviewFilm/{filmId}")]
        public async Task<ActionResult> AddFilmAsync(ReviewDto reviewToCreate, [FromRoute] Guid filmId)
        {
            Log.Information($"AddReviewFilm request received review with ID {reviewToCreate.Id} and film with ID {filmId}");
            await _reviewService.AddToFilmAsync(reviewToCreate, filmId);
            if (reviewToCreate != null)
            {
                Log.Information($"AddReviewFilm response: {reviewToCreate.Id} added to film");
                return Ok("Review added");
            }
            Log.Error("Error adding review");
            return Ok("Error");
        }

        [HttpPut("EditReview")]
        public async Task<ActionResult<ReviewDto>> EditAsync(ReviewDto reviewToUpdate)
        {
            Log.Information($"EditReview request received: {reviewToUpdate}");
            ReviewDto? review = await _reviewService.UpdateAsync(reviewToUpdate);
            if (review != null)
            {
                Log.Information($"EditReview response: {review}");
                return Ok(review);
            }
            Log.Error("EditReview response: Error");
            return Ok("Error");
        }

        [HttpDelete("DeleteReview")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            Log.Information($"DeleteReview request received: {id} ");
            await _reviewService.DeleteAsync(id);
            Log.Information("DeleteReview responce: NoContent");
            return NoContent();
        }
    }
}
