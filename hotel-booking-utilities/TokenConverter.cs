using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities
{
    public static class TokenConverter
    {
        public static string EncodeToken(string token)
        {
            var encodedToken = Encoding.UTF8.GetBytes(token);
            return WebEncoders.Base64UrlEncode(encodedToken);
        }

        public static string DecodeToken(string token)
        {
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            return Encoding.UTF8.GetString(decodedToken);
        }
    }
}
