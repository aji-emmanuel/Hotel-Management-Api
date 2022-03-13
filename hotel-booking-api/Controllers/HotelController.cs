using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.BookingDtos;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.RatingDtos;
using hotel_booking_models;
using hotel_booking_models.Cloudinary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {

        private readonly IHotelService _hotelService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHotelStatisticsService _hotelStatisticsService;
        private readonly IBookingService _bookingService;
        private readonly ILogger _logger;


        public HotelController (ILogger logger,
            IHotelService hotelService,
            UserManager<AppUser> userManager,
            IHotelStatisticsService hotelStatisticsService,
            IBookingService bookingService
            )

        {
            _hotelService = hotelService;
            _userManager = userManager;
            _hotelStatisticsService = hotelStatisticsService;
            _bookingService = bookingService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("all-hotels")]
        public async Task<IActionResult> GetAllHotelsAsync ([FromQuery] PagingDto paging)
        {
            var response = await _hotelService.GetAllHotelsAsync(paging);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpGet("total-hotels-per-location")]
        public async Task<IActionResult> GetTotalHotelsPerLocation ()
        {
            var response = await _hotelService.GetNumberOfHotelsPerLocation();
            return StatusCode(response.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpGet("{hotelId}")]
        public async Task<IActionResult> GetHotelByIdAsync (string hotelId)
        {
            var response = await _hotelService.GetHotelByIdAsync(hotelId);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Policy = Policies.Manager)]
        [HttpPut("{hotelId}")]
        public async Task<IActionResult> UpdateHotelAsync (string hotelId, [FromBody] UpdateHotelDto update)
        {
            var response = await _hotelService.UpdateHotelAsync(hotelId, update);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("{hotelId}/customers")]
        public async Task<IActionResult> GetCustomersByHotelId (string hotelId)
        {
            var response = await _hotelService.GetCustomersByHotelId(hotelId);
            return Ok(response);
        }

        [HttpGet]
        [Route("top-hotels")]
        public async Task<IActionResult> HotelsByRatingsAsync ()
        {
            var response = await _hotelService.GetHotelsByRatingsAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("top-deals")]
        public async Task<IActionResult> TopDealsAsync ()
        {
            var response = await _hotelService.GetTopDealsAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("search/{location}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotelByLocation (string location, [FromQuery] PagingDto paging)
        {
            var result = await _hotelService.GetHotelByLocation(location, paging);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Route("room-by-price")]
        public async Task<IActionResult> GetHotelRoomsByPriceAsync ([FromQuery] PriceDto pricing)
        {
            var response = await _hotelService.GetRoomByPriceAsync(pricing);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("{hotelId}/roomTypes")]
        public async Task<IActionResult> GetHotelRoomTypeAsync ([FromQuery] PagingDto paging, string hotelId)
        {
            var rooms = await _hotelService.GetHotelRoomType(paging, hotelId);
            return Ok(rooms);
        }

        [HttpPost]
        [Route("{hotelId}/addRoomType")]
        public async Task<IActionResult> AddRoomTypeToHotel (string hotelId, [FromBody] RoomTypeRequestDto model)
        {
            var response = await _hotelService.AddRoomTypeToHotel(hotelId, model);
            return Ok(response);
        }

        [HttpGet]
        [Route("{hotelId}/room/{roomTypeId}")]
        public async Task<IActionResult> HotelRoomById (string hotelId, string roomTypeId)
        {
            var room = await _hotelService.GetHotelRooomById(hotelId, roomTypeId);
            return Ok(room);
        }

        [HttpGet]
        [Route("{hotelId}/ratings")]
        public async Task<IActionResult> HotelRatingsAsync (string hotelId)
        {
            var rating = await _hotelService.GetHotelRatings(hotelId);
            return Ok(rating);
        }

        [HttpPost]
        [Authorize(Policy = Policies.Manager)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddHotel ([FromBody] AddHotelDto hotelDto)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var result = await _hotelService.AddHotel(loggedInUser.Id, hotelDto);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{hotelId}/statistics")]
        [Authorize(Policy = Policies.Manager)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotelStatistics (string hotelId)
        {
            _logger.Information($"About Getting statistics for hotel with ID {hotelId}");
            var result = await _hotelStatisticsService.GetHotelStatistics(hotelId);
            _logger.Information($"Gotten stats for hotel with ID {hotelId}");
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Route("{hotelId}/rooms")]
        [Authorize(Policy = Policies.Manager)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddHotelRoom (string hotelId, [FromBody] AddRoomDto roomDto)
        {
            var result = await _hotelService.AddHotelRoom(hotelId, roomDto);
            return StatusCode(result.StatusCode, result);

        }

        [HttpDelete]
        [Route("{hotelId}")]
        [Authorize(Policy = Policies.Manager)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeleteHotelAsync (string hotelId)
        {
            var result = await _hotelService.DeleteHotelByIdAsync(hotelId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Route("{hotelId}/reviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllReviewsByHotel ([FromQuery] PagingDto paging, string hotelId)
        {
            var response = await _hotelService.GetAllReviewsByHotelAsync(paging, hotelId);
            return StatusCode(response.StatusCode, response);
        }


        [HttpPost("book-hotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = Policies.Customer)]
        public async Task<IActionResult> CreateBooking ([FromBody] HotelBookingRequestDto bookingDto)
        {
            string userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _bookingService.Book(userId, bookingDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{hotelId}/top-customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> TopCustomers ([FromRoute] string hotelId)
        {
            var result = await _hotelService.TopHotelCustomers(hotelId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Route("{hotelId}/add-ratings")]
        [Authorize(Policy = Policies.Customer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RateHotel (string hotelId, [FromBody] AddRatingDto rating)
        {
            AppUser user = await _userManager.GetUserAsync(User);

            Response<string> result = await _hotelService.RateHotel(hotelId, user.Id, rating);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("verify-booking")]
        [Authorize(Policy = Policies.Customer)]
        public async Task<IActionResult> VerifyBooking ([FromBody] VerifyBookingDto bookingDto)
        {
            var response = await _bookingService.VerifyBooking(bookingDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("{hotelId}/transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotelTransactions (string hotelId, [FromQuery] PagingDto paging)
        {
            var response = await _hotelService.GetHotelTransaction(hotelId, paging);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Route("hotel-image")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> UploadHotelImage([FromForm] AddHotelAndRoomTypeImageDto model, string hotelId)
        {
            _logger.Information($"Update Image Attempt for hotel with id = {hotelId}");
            var result = await _hotelService.UpdateHotelImage(model, hotelId);
            return Ok(result);
        }

        [HttpPost]
        [Route("roomType-image")]
        [Authorize(Policy = Policies.Manager)]
        public async Task<IActionResult> UploadRoomTypeImage([FromForm] AddImageDto imageDto, string roomTypeId)
        {
            _logger.Information($"Update Image Attempt for hotel with id = {roomTypeId}");
            var result = await _hotelService.UpdateRoomTypeImage(imageDto, roomTypeId);
            return Ok(result);
        }


        [HttpDelete]
        [Route("{roomId}/rooms")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRoomByIdAsync(string roomId)
        {
            var response = await _hotelService.DeleteHotelRoomByIdAsync(roomId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("roomTypedetails/{roomTypeId}")]
        [Authorize(Policy = Policies.Manager)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRoomTypeDetails(string roomTypeId)
        {
            var result = await _hotelService.GetRoomTypeDetails(roomTypeId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
