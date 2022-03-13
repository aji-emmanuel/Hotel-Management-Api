using System.Collections.Generic;

namespace hotel_booking_dto
{
    public class AdminStatisticsDto
    {
        public int TotalNumberOfHotels { get; set; }
        public decimal TotalMonthlyTransactions { get; set; }
        public ICollection<string> Managers { get; set; }
        public decimal Commission { get; set; }
        public decimal TotalMonthlyCommission { get; set; }
        public Dictionary<string, decimal> AnnualRevenue { get; set; }
    }
}
