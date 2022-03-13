using hotel_booking_dto.commons;
using System;
namespace hotel_booking_dto
{
    public class TransactionFilter : PagingDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string SearchQuery { get; set; }
        public TransactionFilter()
        {
            Year = DateTime.Now.Year;
        }
    }
}