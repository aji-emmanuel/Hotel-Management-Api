using System.ComponentModel.DataAnnotations;

namespace hotel_booking_dto.HotelDtos
{
    public class HotelBasicDetailsDto
    {
        [Display(Name = "ManagerId")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }        
        public string Thumbnail { get; set; }
        public double PercentageRating { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
