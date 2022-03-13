using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.BookingDtos
{
    public class BookingResponseDto
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string Hotel { get; set; }
        public string Service { get; set; }
        public DateTime CheckedIn { get; set; }
        public DateTime CheckedOut { get; set; }
    }
}
