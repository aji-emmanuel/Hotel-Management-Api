using FluentValidation;
using hotel_booking_dto.HotelDtos;
using hotel_booking_utilities.ValidatorSettings;

namespace hotel_booking_utilities.Validators.HotelValidators
{
    public class UpdateHotelDtoValidator : AbstractValidator<UpdateHotelDto>
    {
        public UpdateHotelDtoValidator()
        {
            RuleFor(hotel => hotel.Name).NotEmpty().WithMessage("Name cannot be empty")
                .NotNull().WithMessage("Name is required")
                .Matches("[A-Za-z]").WithMessage("Name can only contain alphabeths")
                .MinimumLength(2).WithMessage("Name is limited to a minimum of 2 characters");

            RuleFor(hotel => hotel.Description).NotEmpty().WithMessage("Description cannot be empty")
                .NotNull().WithMessage("Description is required")
                .Matches("[A-Za-z]").WithMessage("Description can only contain alphabeths")
                .MinimumLength(2).WithMessage("Description is limited to a minimum of 2 characters");

            RuleFor(hotel => hotel.Email).EmailAddress();

            RuleFor(hotel => hotel.PhoneNumber).PhoneNumber();

            RuleFor(hotel => hotel.Address).NotEmpty().WithMessage("Address cannot be empty")
                .NotNull().WithMessage("Address is required").Matches("[a-zA-Z0-9]")
                .MinimumLength(2).WithMessage("Address is limited to a minimum of 2 characters");

            RuleFor(hotel => hotel.City).NotEmpty().WithMessage("City cannot be empty")
                .NotNull().WithMessage("City is required").Matches("[a-zA-Z]").WithMessage("City can only contain alphabeths")
                .MinimumLength(2).WithMessage("City is limited to a minimum of 2 characters");

            RuleFor(hotel => hotel.State).NotEmpty().WithMessage("State cannot be empty")
                .NotNull().WithMessage("State is required").Matches("[a-zA-Z]").WithMessage("State can only contain alphabeths")
               .MinimumLength(2).WithMessage("State is limited to a minimum of 2 characters");
        }
    }
}
