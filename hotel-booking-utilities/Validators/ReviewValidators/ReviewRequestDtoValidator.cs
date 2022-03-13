using FluentValidation;
using hotel_booking_dto.ReviewDtos;

namespace hotel_booking_utilities.Validators.ReviewValidators
{
    public class ReviewRequestDtoValidator : AbstractValidator<ReviewRequestDto>
    {
        public ReviewRequestDtoValidator()
        {
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Please input a review");
        }
    }
}
