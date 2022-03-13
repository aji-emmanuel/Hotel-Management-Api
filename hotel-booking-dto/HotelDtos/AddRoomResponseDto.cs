using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.HotelDtos
{
    public class AddRoomResponseDto
    {
        public string Id { get; set; }
        public string RoomNo { get; set; }
        public bool IsBooked { get; set; }
        public string RoomTypeId { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
