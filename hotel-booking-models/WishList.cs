using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace hotel_booking_models

{
    public class WishList
    {
        public string CustomerId { get; set; }
        public string HotelId { get; set; }
        public Customer Customer { get; set; }
        public Hotel Hotel { get; set; }
    }
}