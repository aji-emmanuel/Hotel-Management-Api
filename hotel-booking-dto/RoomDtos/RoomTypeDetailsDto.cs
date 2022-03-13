using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.RoomDtos
{
    public class RoomTypeDetailsDto
    {
        public string Name { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string Thumbnail { get; set; }
        public DateTime DateCreated { get; set; }
        public string PricePerNight { get; set; }
    }
}
