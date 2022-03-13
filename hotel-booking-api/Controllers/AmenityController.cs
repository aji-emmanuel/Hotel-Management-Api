using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;

        public AmenityController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        [HttpGet("{hotelId}")]
        public async Task<IActionResult> GetAmenityByHotelIdAsync(string hotelId)
        {

            var result = await _amenityService.GetAmenityByHotelIdAsync(hotelId);

            if (result is not null)
            {
                return StatusCode(result.StatusCode, result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpPut("{amenityId}")]
        [Authorize(Roles = "Manager")]

        public ActionResult<Response<UpdateAmenityDto>> UpdateAmenity(string amenityId, [FromBody] UpdateAmenityDto update)
        {
            var response = _amenityService.UpdateAmenity(amenityId, update);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]

        public async Task<ActionResult> AddAmenity([FromBody] AddAmenityRequestDto amenity)
        {         
            var response = await _amenityService.AddAmenity(amenity.HotelId, amenity);
            return Ok(response);
        }

        [HttpDelete("{amenityId}")]
        public async Task<ActionResult> DeleteAmenityByIdAsync(string amenityId)
        {
            var response = await _amenityService.DeleteAmenityAsync(amenityId);
            return StatusCode(response.StatusCode, response);
        }
    }
}