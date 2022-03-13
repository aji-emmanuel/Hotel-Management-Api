using System.ComponentModel.DataAnnotations;

namespace hotel_booking_dto.AuthenticationDtos
{
    public class ResetPasswordDto
    {
        public string Token { get; set; }

        public string Email { get; set; }

        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "New Password and Confirm Password must match.")]
        public string ConfirmPassword { get; set; }
    }
}
