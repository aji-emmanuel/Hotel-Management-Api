using System;
using System.Text.Json.Serialization;

namespace hotel_booking_dto.AuthenticationDtos
{
    public class LoginResponseDto
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
