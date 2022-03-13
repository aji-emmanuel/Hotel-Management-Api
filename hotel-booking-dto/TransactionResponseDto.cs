using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto
{
    public class TransactionResponseDto
    {
        public string BookingId { get; set; }
        public string BookingReference { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string ServiceName { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Commission
        {
            get =>  PaymentAmount * (decimal)0.1;
        }            
        public string HotelId { get; set; }
        public string HotelName { get; set; }
        public string CustomerName { get; set; }
        public DateTime PaymentDate { get; set; }     
        
        public DateTime CreatedAt { get; set; }
    }
}
