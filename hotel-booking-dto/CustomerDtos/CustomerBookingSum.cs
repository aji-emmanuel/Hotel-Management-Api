using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.CustomerDtos
{
    public class CustomerBookingSum
    {
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
        public Customer Customer { get; set; }
    }
}
