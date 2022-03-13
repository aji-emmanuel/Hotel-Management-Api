using FluentValidation;
using hotel_booking_dto;

namespace hotel_booking_utilities.Validators.AuthenticationValidators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .NotNull()
                .Matches(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[!*@#$%^&+=]).*$")
                .WithMessage("minimum of 8 character including upper case, number and special character");
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .NotNull()
                .Equal(x => x.NewPassword)
                .WithMessage("New password does not match");
        }
    }
}
