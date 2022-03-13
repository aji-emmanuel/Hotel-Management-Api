using FluentValidation;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_utilities.ValidatorSettings;

namespace hotel_booking_utilities.Validators.AuthenticationValidators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(user => user.FirstName).HumanName();

            RuleFor(user => user.LastName).HumanName();

            RuleFor(user => user.PhoneNumber).PhoneNumber();

            RuleFor(user => user.Email).EmailAddress();

            RuleFor(user => user.Password).Password();

            RuleFor(user => user.Age).GreaterThanOrEqualTo(18).WithMessage("Must be 18 and above");
        }
    }
}
