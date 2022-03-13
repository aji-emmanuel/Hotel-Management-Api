using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using hotel_booking_dto.HotelDtos;

namespace hotel_booking_utilities.Validators.HotelValidators
{
    public class AddRoomDtoValidator : AbstractValidator<AddRoomDto>
    {
        public AddRoomDtoValidator()
        {
            RuleFor(x => x.IsBooked).NotEmpty().Must(x => x == false || x == true).WithMessage("Must be true or false");
            RuleFor(x => x.RoomNo).NotEmpty().WithMessage("This field is required!");
            RuleFor(x => x.RoomTypeId).NotEmpty().WithMessage("This field is required!");
        }
    }
}
