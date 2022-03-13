using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.CustomerDtos
{
    public class TopManagerCustomers
    {
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Gender { get; set; }
        public int NumberOfBookedTimes { get; set; }
        public int TotalAmountSpent { get; set; }
    }
}
