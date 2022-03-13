using FluentValidation;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_utilities.ValidatorSettings;

namespace hotel_booking_utilities.Validators.AuthenticationValidators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(user => user.NewPassword).Password();
            RuleFor(user => user.ConfirmPassword).Password();
            RuleFor(user => user.Email).EmailAddress();
            RuleFor(x => x.NewPassword)
                .Equal(x => x.ConfirmPassword).WithMessage("Password does not match");
        }
    }
}
