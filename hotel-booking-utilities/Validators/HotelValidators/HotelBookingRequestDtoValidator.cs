using FluentValidation;
using hotel_booking_dto.HotelDtos;
using System;

namespace hotel_booking_utilities.Validators.HotelValidators
{
    public class HotelBookingRequestDtoValidator : AbstractValidator<HotelBookingRequestDto>
    {
        public HotelBookingRequestDtoValidator()
        {
            RuleFor(hbDto => hbDto.CheckIn)
                .NotEmpty().WithMessage("CheckIn Must not be Empty")
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("CheckIn Date has passed")
                .LessThan(x => x.CheckOut).WithMessage("CheckIn Date has to be Lesser than CheckOut Date");
            RuleFor(hbDto => hbDto.CheckOut)
                .NotEmpty().WithMessage("CheckOut Must not be Empty")
                .GreaterThan(x => x.CheckIn).WithMessage("CheckOut Date has to be Greater that CheckIn Date");


        }
    }
}
