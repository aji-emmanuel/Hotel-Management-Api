using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.HotelDtos
{
    public class AddHotelAndRoomTypeImageDto
    {
        public IFormFile File { get; set; }
    }
}
