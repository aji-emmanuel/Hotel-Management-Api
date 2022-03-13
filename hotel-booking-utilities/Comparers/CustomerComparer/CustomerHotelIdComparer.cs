using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace hotel_booking_utilities.Comparer
{
    public class CustomerHotelIdComparer : IEqualityComparer<Customer>
    {
        public  bool Equals(Customer x, Customer y)
        {
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            if (x.AppUserId == y.AppUserId) return true;
            
            return false;
        }

        public int GetHashCode([DisallowNull] Customer obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            return obj.AppUserId.GetHashCode();
        }
    }
}
