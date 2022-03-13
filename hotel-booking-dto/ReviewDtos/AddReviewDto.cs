using System.ComponentModel.DataAnnotations;

namespace hotel_booking_dto.ReviewDtos
{
    public class AddReviewDto
    {
        [DataType(DataType.Text)]
        public string Comment { get; set; }
        public string HotelId { get; set; }


    }
}
