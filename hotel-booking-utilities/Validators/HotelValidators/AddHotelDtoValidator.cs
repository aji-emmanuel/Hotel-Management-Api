using FluentValidation;
using hotel_booking_dto.HotelDtos;
using hotel_booking_utilities.ValidatorSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities.Validators.hotelValidators
{
    public class AddHotelDtoValidator : AbstractValidator<AddHotelDto>
    {
        public AddHotelDtoValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress();
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Hotel address is required")
                .NotNull().WithMessage("Hotel address is required");

            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Hotel name is required");
            RuleFor(x => x.State)
                .NotEmpty()
                .NotNull()
                .WithMessage("State is required");
            RuleFor(x => x.City)
                .NotEmpty()
                .NotNull()
                .WithMessage("City is required");
            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull()
                .WithMessage("Description is required");

            RuleFor(x => x.Phone).PhoneNumber();
        }
    }
}
