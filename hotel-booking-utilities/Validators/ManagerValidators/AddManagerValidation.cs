using FluentValidation;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_utilities.ValidatorSettings;

namespace hotel_booking_utilities.Validators.ManagerValidators
{
    public class AddManagerValidation : AbstractValidator<ManagerDto>
    {
        public AddManagerValidation()
        {
            RuleFor(x => x.BusinessEmail).EmailAddress();
            RuleFor(x => x.HotelEmail).EmailAddress();
            RuleFor(x => x.AccountName).HumanName();
            RuleFor(x => x.AccountNumber).NotNull().NotEmpty().Length(10);
            RuleFor(x => x.Age).NotNull().NotEmpty().GreaterThanOrEqualTo(18);
            RuleFor(x => x.BusinessPhone).PhoneNumber();
            RuleFor(x => x.CompanyAddress).Address();
            RuleFor(x => x.FirstName).HumanName();
            RuleFor(x => x.LastName).HumanName();
            RuleFor(x => x.State).State();
            RuleFor(x => x.Password).Password();
            RuleFor(x => x.HotelState).State();
            RuleFor(x => x.HotelPhone).PhoneNumber();
            RuleFor(x => x.HotelName).NotEmpty();
            RuleFor(x => x.HotelDescription).NotEmpty();
            RuleFor(x => x.HotelCity).NotEmpty();
            RuleFor(x => x.HotelAddress).Address();
            RuleFor(x => x.Gender).Gender();
            RuleFor(x => x.CompanyName).NotNull().NotEmpty();

        }
    }
}
