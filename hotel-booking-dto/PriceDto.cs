using hotel_booking_dto.commons;

namespace hotel_booking_dto
{
    public class PriceDto : PagingDto
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
