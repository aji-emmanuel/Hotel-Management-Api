using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities.Exceptions
{
    public class PaymentException : Exception
    {
        public PaymentException()
        {

        }
        public PaymentException(string message) : base(message)
        {

        }
    }
}
