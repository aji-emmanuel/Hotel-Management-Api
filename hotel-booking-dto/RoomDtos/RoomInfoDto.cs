using hotel_booking_dto.commons;
using hotel_booking_models;

namespace hotel_booking_dto.RoomDtos
{
    public class RoomInfoDto
    {
        public string HotelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Thumbnail { get; set; }
        public string HotelName { get; set; }
        public decimal DiscountPrice { get; set; }
    }
}
