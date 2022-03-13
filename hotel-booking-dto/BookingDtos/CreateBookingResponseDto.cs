using System;

namespace hotel_booking_dto.BookingDtos
{
    public class CreateBookingResponseDto
    {
        public string BookReference { get; set; }
        public int RoomNo { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int NumberOfPeople { get; set; }
        public string ServiceName { get; set; }
        public string Hotel { get; set; }
        public string RoomType { get; set; }
        public bool PaymentStatus { get; set; }
        public decimal Price { get; set; }
        public string PaymentReference { get; set; }
        public string PaymentUrl { get; set; }
    }
}
