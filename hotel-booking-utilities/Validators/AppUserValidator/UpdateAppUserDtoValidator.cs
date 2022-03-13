using FluentValidation;
using hotel_booking_dto.AppUserDto;
using hotel_booking_utilities.ValidatorSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities.Validators.AppUserValidator
{
    public class UpdateAppUserDtoValidator : AbstractValidator<UpdateAppUserDto>
    {
        public UpdateAppUserDtoValidator()
        {
            RuleFor(user => user.FirstName).HumanName();
            RuleFor(user => user.LastName).HumanName();
            RuleFor(user => user.PhoneNumber).PhoneNumber();
            RuleFor(user => user.Age).GreaterThanOrEqualTo(18).WithMessage("Must be 18 and above");
        }
    }
}
