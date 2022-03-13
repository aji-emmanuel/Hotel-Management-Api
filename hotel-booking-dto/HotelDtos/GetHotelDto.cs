using hotel_booking_dto.ReviewDtos;
using hotel_booking_dto.RoomDtos;
using System.Collections.Generic;

namespace hotel_booking_dto.HotelDtos
{
    /// <summary>
    /// Model to be contained in Data field of Get an hotel response
    /// </summary>
    public class GetHotelDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public double Rating { get; set; }
        public int NumberOfReviews { get; set; }
        public string FeaturedImage { get; set; }
        public IEnumerable<string> Gallery { get; set; }
        public IEnumerable<RoomTypeDto> RoomTypes { get; set; }
        public IEnumerable<AmenityDto> Amenities { get; set; }
        public IEnumerable<ReviewDto> Reviews { get; set; }
    }
}
