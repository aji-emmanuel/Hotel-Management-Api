using FluentValidation;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_utilities.ValidatorSettings;

namespace hotel_booking_utilities.Validators.AuthenticationValidators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(user => user.Email).EmailAddress();

            RuleFor(user => user.Password).Password();
        }
    }
}
