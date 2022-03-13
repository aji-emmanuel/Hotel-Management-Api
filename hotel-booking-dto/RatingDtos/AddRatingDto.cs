using System.ComponentModel.DataAnnotations;

namespace hotel_booking_dto.RatingDtos
{
    public class AddRatingDto
    {
        [Required]
        [Range(1,5, ErrorMessage =("Rating must be between 1 and 5 inclusive"))]
        public int Ratings { get; set; }
    }
}
