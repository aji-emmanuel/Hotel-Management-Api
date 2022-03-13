using FluentValidation;
using hotel_booking_dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities.Validators.ManagerValidators
{
    public class ManagerRequestValidator : AbstractValidator<ManagerRequestDto>
    {
        public ManagerRequestValidator()
        {
            RuleFor(x => x.HotelName)
                .NotEmpty().WithMessage("Hotel name is required");
            RuleFor(x => x.Email)
                .EmailAddress();
        }
    }
}
