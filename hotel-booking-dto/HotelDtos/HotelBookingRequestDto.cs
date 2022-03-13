using System;
using System.ComponentModel.DataAnnotations;

namespace hotel_booking_dto.HotelDtos
{
    public class HotelBookingRequestDto
    {
        [Required]
        public string RoomId { get; set; }
        [Required]

        public DateTime CheckIn { get; set; }
        [Required]
        public DateTime CheckOut { get; set; }
        public int NoOfPeople { get; set; }
        [Required]
        public string PaymentService { get; set; }
    }
}
