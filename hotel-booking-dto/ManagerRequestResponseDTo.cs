using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto
{
    public class ManagerRequestResponseDTo
    {
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string Email { get; set; }
        public bool ConfirmationFlag { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
