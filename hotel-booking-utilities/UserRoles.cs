using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities
{
    /// <summary>
    /// This specifies the roles available within the application
    /// </summary>
    public class UserRoles
    {
        public const string Admin = "Admin";
  
        public const string HotelManager = "Manager";
  
        public const string Customer = "Customer";
    }

    public class Payments
    {
        public const string Success = "success";
        public const string Successful = "successful";
        public const string Paystack = "paystack";
        public const string Flutterwave = "flutterwave";
    }

    public class PaymentStatus
    {
        public const string Successful = "Successful";
        public const string Pending = "Pending";
        public const string Failed = "Failed";
    }
}
