using FluentValidation;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_utilities.ValidatorSettings;

namespace hotel_booking_utilities.Validators.AuthenticationValidators
{
    public class UpdatePasswordDtoValidator : AbstractValidator<UpdatePasswordDto>
    {
        public UpdatePasswordDtoValidator()
        {
            RuleFor(user => user.Email).EmailAddress();
            RuleFor(user => user.CurrentPassword).Password();
            RuleFor(user => user.NewPassword).Password();
            RuleFor(user => user.ConfirmPassword).Password();
        }
    }
}
