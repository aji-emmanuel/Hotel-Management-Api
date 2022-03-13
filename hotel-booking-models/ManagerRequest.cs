using System;

namespace hotel_booking_models
{
    public class ManagerRequest : BaseEntity
    {
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool ConfirmationFlag { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
