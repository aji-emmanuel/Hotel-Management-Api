namespace hotel_booking_dto.CustomerDtos
{
    public class TopCustomerDto
    {
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public decimal TotalBookingAmount { get; set; }
    }
}
