using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.BookingDtos
{
    public class VerifyBookingDto
    {
        [Required]
        public string TransactionReference { get; set; }
        public string TransactionId { get; set; }
    }
}
