using FluentValidation;
using hotel_booking_dto.HotelDtos;

namespace hotel_booking_utilities.Validators.hotelValidators
{
    public class AddHotelRoomDtoValidator : AbstractValidator<AddRoomDto>
    {
        public AddHotelRoomDtoValidator()
        {
            RuleFor(x => x.RoomNo).NotNull().NotEmpty().WithMessage("Room number is required");

            RuleFor(x => x.RoomTypeId)
                .NotEmpty().WithMessage("Room type id is required")
                .NotNull().WithMessage("Room type id is required");
            RuleFor(x => x.IsBooked)
                .NotEmpty()
                .NotNull();
        }
    }
}