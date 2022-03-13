namespace hotel_booking_dto.HotelDtos
{

    public class HotelBasicDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public double Rating { get; set; }
        public string FeaturedImage { get; set; }
        public int NumberOfReviews { get; set; }
        public string  ManagerId { get; set; }
    }
}
