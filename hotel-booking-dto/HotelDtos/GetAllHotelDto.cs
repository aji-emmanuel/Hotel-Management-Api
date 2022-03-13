using hotel_booking_dto.RoomDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.HotelDtos
{
    /// <summary>
    /// Model to be contained in the Data field of Get All Hotels Response
    /// </summary>
    public class GetAllHotelDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public double Rating { get; set; }
        public string FeaturedImage { get; set; }
        public IEnumerable<string> Gallery { get; set; }
        public IEnumerable<RoomTypeDto> RoomTypes { get; set; }
    }
}
