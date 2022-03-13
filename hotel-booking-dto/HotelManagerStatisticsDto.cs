using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto
{
    public class HotelManagerStatisticsDto
    {
        public int TotalHotels { get; set; }
        public double AverageHotelRatings { get; set; }
        public int TotalNumberOfCustomers { get; set; }
        public int TotalRooms { get; set; }
        public int AvailableRooms { get; set; }
        public int BookedRooms { get; set; }
        public decimal TotalMonthlyTransactions { get; set; }
        public Dictionary<string, decimal> TotalAnnualRevenue { get; set; }
    }
}
