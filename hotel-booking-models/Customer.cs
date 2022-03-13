using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_models
{
    public class Customer
    {
        [Key]
        public string AppUserId { get; set; }
        public string CreditCard { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<WishList> WishLists { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
