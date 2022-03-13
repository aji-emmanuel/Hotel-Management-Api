using FluentValidation;

using hotel_booking_dto.ManagerDtos;
using hotel_booking_utilities.ValidatorSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities.Validators.ManagerValidators
{
    public class UpdateManagerDtoValidator : AbstractValidator<UpdateManagerDto>
    {
        public UpdateManagerDtoValidator()
        {
            RuleFor(manager => manager.FirstName).HumanName();
            RuleFor(manager => manager.LastName).HumanName();
            RuleFor(manager => manager.PhoneNumber).PhoneNumber();
            RuleFor(manager => manager.Age).GreaterThanOrEqualTo(18).WithMessage("Must be 18 and above"); ;
            RuleFor(manager => manager.CompanyName).HumanName();
            RuleFor(manager => manager.BusinessEmail).EmailAddress();
            RuleFor(manager => manager.BusinessPhone).PhoneNumber();
            RuleFor(manager => manager.CompanyAddress).Address();
            RuleFor(manager => manager.State).State();
            RuleFor(manager => manager.AccountName).HumanName();
            RuleFor(manager => manager.AccountNumber).NotNull()
                .WithMessage("Account number is required")
                .Matches(@"\d{10}$").WithMessage("Account number must be a 10-digit number"); 


        }
    }
}
