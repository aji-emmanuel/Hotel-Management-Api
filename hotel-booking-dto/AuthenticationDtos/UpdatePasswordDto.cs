namespace hotel_booking_dto.AuthenticationDtos
{
    public class UpdatePasswordDto
    {
        public string Email { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
