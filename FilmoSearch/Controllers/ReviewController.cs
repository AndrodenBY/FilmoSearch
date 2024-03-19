using FilmoSearch.DTO;
using FilmoSearch.Services.Actor;
using FilmoSearch.Services.Review;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FilmoSearch.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private ReviewService _reviewService;
        public ReviewController(ReviewService reviewService) { _reviewService = reviewService; }

        [HttpGet("GetReviews")]
        public ActionResult<IEnumerable<ReviewDto>> Get()
        {
            Log.Information("Getting all reviews");
            return Ok(_reviewService.GetAll());
        }

        [HttpGet("GetReviewById/{id}")]
        public ActionResult<ReviewDto> GetById([FromRoute]Guid id)
        {
            Log.Information($"Getting film by ID {id}");
            ReviewDto? review = _reviewService.GetById(id);
            if (review != null)
            {
                Log.Information($"Review found: {review}");
                return Ok(review);
            }
            Log.Warning($"Review with ID {id} not found");
            return Ok("Error");
        }

        [HttpPost("AddReview")]
        public ActionResult<ReviewDto> Add(ReviewDto reviewToCreate)
        {
            Log.Information($"AddReview request received: {reviewToCreate}");
            ReviewDto? review = _reviewService.Create(reviewToCreate);
            if (review != null)
            {
                Log.Information($"AddReview response: {review}");
                return Ok(review);
            }
            Log.Error("AddReview responce: Error");
            return Ok("Error");
        }

        [HttpPost("AddReviewFilm/{filmId}")]
        public ActionResult AddFilm(ReviewDto reviewToCreate, [FromRoute] Guid filmId)
        {
            Log.Information($"AddReviewFilm request received review with ID {reviewToCreate.Id} and film with ID {filmId}");
            _reviewService.AddToFilm(reviewToCreate, filmId);
            if (reviewToCreate != null)
            {
                Log.Information($"AddReviewFilm response: {reviewToCreate.Id} added to film");
                return Ok("Review added");
            }
            Log.Error("Error adding review");
            return Ok("Error");
        }

        [HttpPut("EditReview")]
        public ActionResult<ReviewDto> Edit(ReviewDto reviewToUpdate)
        {
            Log.Information($"EditReview request received: {reviewToUpdate}");
            ReviewDto? review = _reviewService.Update(reviewToUpdate);
            if (review != null)
            {
                Log.Information($"EditReview response: {review}");
                return Ok(review);
            }
            Log.Error("EditReview response: Error");
            return Ok("Error");
        }

        [HttpDelete("DeleteReview")]
        public ActionResult Delete(Guid id)
        {
            Log.Information($"DeleteReview request received: {id} ");
            _reviewService.Delete(id);
            Log.Information("DeleteReview responce: NoContent");
            return NoContent();
        }
    }
}
