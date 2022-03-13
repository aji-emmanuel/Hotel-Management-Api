using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.BookingDtos
{
    public class GetBookingResponseDto
    {
        public string Id { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public decimal Price { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Hotel { get; set; }
        public string BookingReference { get; set; }
        public bool PaymentStatus { get; set; }
    }
}
