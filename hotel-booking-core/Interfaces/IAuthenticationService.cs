using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using System.Threading.Tasks;
using hotel_booking_dto.RefereshTokenDto;
using hotel_booking_dto.TokenDto;
using Microsoft.AspNetCore.Mvc;

namespace hotel_booking_core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Response<string>> Register(RegisterUserDto userDto);
        Task<Response<LoginResponseDto>> Login(LoginDto loginDto);
        Task<Response<string>> ConfirmEmail(ConfirmEmailDto confirmEmailDto);
        Task<Response<string>> ForgotPassword(string email);
        Task<Response<bool>> ChangePassword(string id, ChangePasswordDto changePasswordDto);
        Task<Response<string>> ResetPassword(ResetPasswordDto model);
        Task<Response<string>> UpdatePassword(UpdatePasswordDto model);
        Task<Response<RefreshTokenToReturnDto>> RefreshToken(RefreshTokenRequestDto token);
        Task<bool> ValidateUserRole(string userId, string[] roles);
    }
}
