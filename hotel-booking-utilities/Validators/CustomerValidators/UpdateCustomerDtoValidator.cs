using FluentValidation;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_utilities.ValidatorSettings;

namespace hotel_booking_utilities.Validators.CustomerValidators
{
    public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerDtoValidator()
        {
            RuleFor(customer => customer.FirstName).HumanName();
            RuleFor(customer => customer.LastName).HumanName();
            RuleFor(customer => customer.PhoneNumber).PhoneNumber();
            RuleFor(customer => customer.Age).GreaterThanOrEqualTo(18).WithMessage("Must be 18 and above"); ;
            RuleFor(customer => customer.CreditCard).CreditCard();
            RuleFor(customer => customer.Address).Address();
            RuleFor(customer => customer.State).State();         

        }
    }
}
