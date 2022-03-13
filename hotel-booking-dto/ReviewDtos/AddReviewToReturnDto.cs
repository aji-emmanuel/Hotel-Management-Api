using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.ReviewDtos
{
    public class AddReviewToReturnDto
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public string CustomerId { get; set; }
        public string HotelId { get; set; }
        public string CreatedAt { get; set; }
    }
}
