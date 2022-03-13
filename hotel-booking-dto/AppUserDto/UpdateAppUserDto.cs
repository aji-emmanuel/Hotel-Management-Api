using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.AppUserDto
{
    public class UpdateAppUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string CreditCard { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
