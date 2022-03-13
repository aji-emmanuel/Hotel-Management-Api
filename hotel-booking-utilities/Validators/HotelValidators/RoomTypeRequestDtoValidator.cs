using FluentValidation;
using hotel_booking_dto.HotelDtos;

namespace hotel_booking_utilities.Validators.HotelValidators
{
    public class RoomTypeRequestDtoValidator : AbstractValidator<RoomTypeRequestDto>
    {
        public RoomTypeRequestDtoValidator ()
        {
            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Price).NotNull().NotEmpty().GreaterThan(0).WithMessage("Provide a valid amount");
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("RoomType requires a name");
        }
    }
}
