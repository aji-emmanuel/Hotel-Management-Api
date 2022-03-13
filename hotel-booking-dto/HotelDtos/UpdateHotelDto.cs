
namespace hotel_booking_dto.HotelDtos
{
    /// <summary>
    /// Model to be contained in the Data field of Update hotel response
    /// </summary>
    public class UpdateHotelDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
