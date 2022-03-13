using FluentValidation;
using hotel_booking_core.Interface;
using hotel_booking_core.Interfaces;
using hotel_booking_core.Services;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.Repositories.Implementations;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_data.UnitOfWork.Implementation;
using hotel_booking_dto;
using hotel_booking_dto.AppUserDto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.RatingDtos;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_utilities;
using hotel_booking_utilities.EmailBodyHelper;
using hotel_booking_utilities.HttpClientService.Implementation;
using hotel_booking_utilities.HttpClientService.Interface;
using hotel_booking_utilities.PaymentGatewaySettings;
using hotel_booking_utilities.Validators.AppUserValidator;
using hotel_booking_utilities.Validators.AuthenticationValidators;
using hotel_booking_utilities.Validators.CustomerValidators;
using hotel_booking_utilities.Validators.HotelValidators;
using hotel_booking_utilities.Validators.ReviewValidators;
using hotel_booking_utilities.Validators.ManagerValidators;
using Microsoft.Extensions.DependencyInjection;
using hotel_booking_utilities.Validators.AdminValidators;
using hotel_booking_dto.ManagerDtos;

namespace hotel_booking_api.Extensions
{
    public static class DIServiceExtension
    {
        public static void AddDependencyInjection (this IServiceCollection services)
        {
            // Add Service Injections Here
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddScoped<IAdminService, AdminService>();



            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<IAmenityService, AmenityService>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddTransient<IManagerService, ManagerService>();

            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IWishListService, WishListService>();


            services.AddScoped<IHotelStatisticsService, HotelStatisticsService>();

            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();


            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<PaystackPaymentHandler>();
            services.AddScoped<FlutterwavePaymentHandler>();


            services.AddScoped<IHttpClientService, HttpClientService>();


            // Add Repository Injections Here
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Add Model Services Injection Here

            services.AddScoped<IAmenityRepository, AmenityRepository>();

            // Add Fluent Validator Injections Here
            services.AddTransient<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddTransient<IValidator<LoginDto>, LoginDtoValidator>();

            services.AddTransient<IValidator<ManagerRequestDto>, ManagerRequestValidator>();

            services.AddTransient<IValidator<UpdateHotelDto>, UpdateHotelDtoValidator>();
            services.AddTransient<IValidator<UpdatePasswordDto>, UpdatePasswordDtoValidator>();
            services.AddTransient<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
            services.AddTransient<IValidator<ReviewRequestDto>, ReviewRequestDtoValidator>();

            services.AddTransient<IValidator<UpdateAppUserDto>, UpdateAppUserDtoValidator>();
            services.AddTransient<IValidator<UpdateCustomerDto>, UpdateCustomerDtoValidator>();
            services.AddTransient<IValidator<HotelBookingRequestDto>, HotelBookingRequestDtoValidator>();
            services.AddTransient<IValidator<AddRatingDto>, AddRatingDtoValidator>();
            services.AddTransient<IValidator<TransactionFilter>, TransactionFilterValidator>();
            services.AddTransient<IValidator<RoomTypeRequestDto>, RoomTypeRequestDtoValidator>();
            services.AddTransient<IValidator<ChangePasswordDto>, ChangePasswordValidator>();
            services.AddTransient<IValidator<ManagerDto>, AddManagerValidation>();
        }
    }
}

