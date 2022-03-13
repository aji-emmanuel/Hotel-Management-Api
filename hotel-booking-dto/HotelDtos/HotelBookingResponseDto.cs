using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.HotelDtos
{
    public class HotelBookingResponseDto
    {
        public string BookingReference { get; set; }
        public int RoomNo { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int NoOfPeople { get; set; }
        public string ServiceName { get; set; }
        public string Hotel { get; set; }
        public string RoomType { get; set; }
        public bool PaymentStatus { get; set; }
        public decimal Price { get; set; }
        public string PaymentReference { get; set; }
    }
}
