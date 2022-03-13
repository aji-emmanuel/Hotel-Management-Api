using hotel_booking_dto;
using hotel_booking_dto.BookingDtos;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_utilities.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IBookingService
    {
        Task<Response<PageResult<IEnumerable<GetBookingResponseDto>>>> GetCustomerBookings(string userId, PagingDto paging);
        Task<Response<HotelBookingResponseDto>> Book(string userId, HotelBookingRequestDto bookingDto);
        Task<Response<string>> VerifyBooking(VerifyBookingDto bookingDto);
    }
}
