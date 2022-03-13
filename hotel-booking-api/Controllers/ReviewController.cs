using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.ReviewDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Serilog;


namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReviewController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService, ILogger logger)
        {
            _reviewService = reviewService;
            _logger = logger;

        }

        [HttpPatch("reviewId")]
        public IActionResult UpdateCustomerReview([FromQuery] string reviewId, [FromBody] ReviewRequestDto review)
        {
            var customerId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
           
            var response = _reviewService.UpdateUserReview(customerId, reviewId, review);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Route("add-reviews")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddReviews([FromBody] AddReviewDto model)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _reviewService.AddReviewAsync(model, customerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete]
        public ActionResult<Response<string>> DeleteReviews(string reviewId)
        {
            _logger.Information($"About deleting review with ID {reviewId}");

            var customerId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var response = _reviewService.DeleteUserReview(customerId, reviewId);
            _logger.Information($"Succesfully deleted review with ID {reviewId}");

            return Ok(response);

        }
    }
}
