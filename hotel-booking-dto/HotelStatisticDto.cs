using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto
{
    public class HotelStatisticDto
    {
        public string Name { get; set; }
        public int NumberOfRooms { get; set; }
        public double AverageRatings { get; set; }
        public int RoomsOccupied { get; set; }
        public int RoomsUnOccupied { get; set; }
        public int NumberOfReviews { get; set; }
        public int TotalNumberOfBookings { get; set; }
        public decimal TotalEarnings { get; set; }
        public int RoomTypes { get; set; }
        public int NumberOfAmenities { get; set; }

    }
}
