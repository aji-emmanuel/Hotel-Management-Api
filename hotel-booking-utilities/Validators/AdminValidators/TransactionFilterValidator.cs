using FluentValidation;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;

namespace hotel_booking_utilities.Validators.AdminValidators
{
    public class TransactionFilterValidator : AbstractValidator<TransactionFilter>
    {
        public TransactionFilterValidator()
        {
            RuleFor(Transaction => Transaction.Month).GreaterThanOrEqualTo(0).LessThan(13).WithMessage("Month must be a number between 1 and 12");
            RuleFor(Transaction => Transaction.Year.ToString()).MaximumLength(4).MinimumLength(4).WithMessage("Invalid Year");

        }
    }
}
