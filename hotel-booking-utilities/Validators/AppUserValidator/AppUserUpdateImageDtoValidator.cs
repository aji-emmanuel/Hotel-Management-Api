using FluentValidation;
using hotel_booking_models.Cloudinary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities.Validators.AppUserValidator
{
    class AppUserUpdateImageDtoValidator : AbstractValidator<AddImageDto>
    {
        public AppUserUpdateImageDtoValidator()
        {
            RuleFor(img => img.Image)
                .NotEmpty().WithMessage("please image can't be empty")
                .NotNull().WithMessage("please image can't be empty");
        }
    }
}
