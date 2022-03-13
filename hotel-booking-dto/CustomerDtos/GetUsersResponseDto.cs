using System;

namespace hotel_booking_dto.CustomerDtos
{
    public class GetUsersResponseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
