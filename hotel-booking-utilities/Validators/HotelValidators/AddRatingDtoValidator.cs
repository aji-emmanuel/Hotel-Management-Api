using FluentValidation;
using hotel_booking_dto.RatingDtos;

namespace hotel_booking_utilities.Validators.HotelValidators
{
    public class AddRatingDtoValidator: AbstractValidator<AddRatingDto>
    {
        public AddRatingDtoValidator()
        {
            RuleFor(rate => rate.Ratings)
                .InclusiveBetween(1, 5).WithMessage("Rating value must be between 1 - 5 inclusive");
        }
    }
}
