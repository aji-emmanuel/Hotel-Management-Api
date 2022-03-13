using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IAmenityService
    {
        Task<Response<IEnumerable<AmenityDto>>> GetAmenityByHotelIdAsync(string hotelId);
        Response<UpdateAmenityDto> UpdateAmenity(string id, UpdateAmenityDto update);
        Task<Response<AddAmenityResponseDto>> AddAmenity(string id, AddAmenityRequestDto model);
        Task<Response<string>> DeleteAmenityAsync(string amenityId);
    }
}
