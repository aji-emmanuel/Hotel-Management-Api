using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.RatingDtos
{
    public class RatingDto
    {
        public string HotelId { get; set; }
        public int Ratings { get; set; }
        public string CustomerId { get; set; }
    }
}
