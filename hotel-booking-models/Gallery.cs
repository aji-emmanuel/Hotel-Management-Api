using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_models
{
    public class Gallery : BaseEntity
    {
        public string HotelId { get; set; }
        public string ImageUrl { get; set; } 
        public bool IsFeature { get; set; }
        public Hotel Hotel { get; set; }
    }
}
