using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities
{
    public static class ReferenceGen
    {
        /// <summary>
        /// Generates a randowm number
        /// </summary>
        /// <returns>integer of randowm number</returns>
        public static int Generate()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(100000000, 999999999);
        }

        public static string GetInitials(string word)
        {
            var split = word.Split(" ");
            if(split.Length < 2)
            {
                return word.Substring(0, 3);
            }
            string result = String.Empty;
            foreach (var item in split)
            {
                result += item[0];
            }
            return result;
        }
    }
}
