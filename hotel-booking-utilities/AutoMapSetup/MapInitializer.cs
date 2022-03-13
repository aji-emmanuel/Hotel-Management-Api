using AutoMapper;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_dto.AppUserDto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.BookingDtos;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_dto.RatingDtos;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_dto.RoomDtos;
using hotel_booking_models;
using System.Linq;

namespace hotel_booking_utilities.AutoMapSetup
{
    public class MapInitializer : Profile
    {
        public MapInitializer ()
        {
            // Authentication Maps
            CreateMap<AppUser, RegisterUserDto>().ReverseMap();
            CreateMap<AppUser, RegisterUserDto>().ReverseMap();
            CreateMap<AppUser, LoginDto>().ReverseMap();
            CreateMap<AppUser, ResetPasswordDto>().ReverseMap();
            CreateMap<AppUser, UpdatePasswordDto>().ReverseMap();
            CreateMap<AppUser, UpdateAppUserDto>().ReverseMap();

            // Amenity Maps
            CreateMap<Amenity, UpdateAmenityDto>().ReverseMap();
            CreateMap<Amenity, AddAmenityRequestDto>().ReverseMap();
            CreateMap<Amenity, AddAmenityResponseDto>().ReverseMap();
            CreateMap<Amenity, AmenityDto>().ReverseMap();

            // Booking Maps
            CreateMap<Booking, HotelBookingRequestDto>().ReverseMap();
            CreateMap<Booking, HotelBookingResponseDto>()
                .ForMember(x => x.Price, y => y.MapFrom(src => src.Room.Roomtype.Price - (src.Room.Roomtype.Price * src.Room.Roomtype.Discount)))
                .ForMember(x => x.RoomType, y => y.MapFrom(src => src.Room.Roomtype.Name))
                .ForMember(x => x.RoomNo, y => y.MapFrom(src => src.Room.RoomNo))
                .ForMember(x => x.PaymentReference, y => y.MapFrom(src => src.Payment.TransactionReference))
                .ForMember(x => x.PaymentStatus, y => y.MapFrom(src => src.PaymentStatus))
                .ForMember(x => x.Hotel, y => y.MapFrom(src => src.Room.Roomtype.Hotel.Name))
                .ForMember(x => x.Price, y => y.MapFrom(src => src.Room.Roomtype.Price));
            CreateMap<Booking, GetBookingResponseDto>()
                .ForMember(x => x.RoomType, y => y.MapFrom(src => src.Room.Roomtype.Name))
                .ForMember(x => x.Hotel, y => y.MapFrom(src => src.Hotel.Name))
                .ForMember(x => x.RoomNumber, y => y.MapFrom(src => src.Room.RoomNo))
                .ForMember(x => x.Price, y => y.MapFrom(src => src.Room.Roomtype.Price));
            CreateMap<Booking, BookingResponseDto>()
                .ForMember(x => x.Hotel, y => y.MapFrom(src => src.Hotel.Name))
                .ForMember(x => x.CustomerName, y => y.MapFrom(src => $"{src.Customer.AppUser.FirstName} {src.Customer.AppUser.LastName}"));
            CreateMap<Customer, TopCustomerDto>()
                .ForMember(x => x.FirstName, y => y.MapFrom(src => src.AppUser.FirstName))
                .ForMember(x => x.LastName, y => y.MapFrom(src => src.AppUser.LastName))
                .ForMember(x => x.Email, y => y.MapFrom(src => src.AppUser.Email))
                .ForMember(x => x.Avatar, y => y.MapFrom(src => src.AppUser.Avatar))
                .ForMember(x => x.Gender, y => y.MapFrom(src => src.AppUser.Gender))
                .ForMember(x => x.Age, y => y.MapFrom(src => src.AppUser.Age));

            // Hotel Maps
            CreateMap<Hotel, HotelBasicDetailsDto>()
                .ForMember(x => x.Thumbnail, y => y.MapFrom(src => src.Galleries.FirstOrDefault(opt => opt.IsFeature).ImageUrl))
                .ForMember(x => x.PercentageRating, y => y.MapFrom(src => src.Ratings.Count == 0 ? 100 : (double)src.Ratings.Sum(r => r.Ratings) * 100 / ((double)src.Ratings.Count * 5)))
                .ForMember(x => x.Price, y => y.MapFrom(src => src.RoomTypes.OrderBy(rt => rt.Price).FirstOrDefault().Price));
            CreateMap<Booking, HotelCustomersDto>()
                .ForMember(x => x.FirstName, y => y.MapFrom(src => src.Customer.AppUser.FirstName))
                .ForMember(x => x.LastName, y => y.MapFrom(src => src.Customer.AppUser.LastName))
                .ForMember(x => x.Gender, y => y.MapFrom(src => src.Customer.AppUser.Gender))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(src => src.Customer.AppUser.PhoneNumber))
                .ForMember(x => x.State, y => y.MapFrom(src => src.Customer.State))
                .ForMember(x => x.Age, y => y.MapFrom(src => src.Customer.AppUser.Age))
                .ForMember(x => x.Email, y => y.MapFrom(src => src.Customer.AppUser.Email));




            CreateMap<RoomType, RoomInfoDto>()
                .ForMember(x => x.HotelName, y => y.MapFrom(c => c.Hotel.Name))
                .ForMember(x => x.DiscountPrice, y => y.MapFrom(c => c.Discount));

            CreateMap<UpdateHotelDto, Hotel>().ReverseMap();

            CreateMap<Hotel, HotelBasicDto>()
                .ForMember(x => x.FeaturedImage, y => y.MapFrom(src => src.Galleries.FirstOrDefault(opt => opt.IsFeature).ImageUrl))
                .ForMember(x => x.Rating, y => y.MapFrom(src => src.Ratings.Count == 0 ? 5 : (double)src.Ratings.Sum(r => r.Ratings) / ((double)src.Ratings.Count)))
                .ForMember(x => x.NumberOfReviews, y => y.MapFrom(src => src.Reviews.Count));

            CreateMap<GalleryDto, Gallery>().ReverseMap();
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
            CreateMap<Hotel, AddHotelDto>().ReverseMap();
            CreateMap<Hotel, AddHotelResponseDto>().ReverseMap();
            CreateMap<Hotel, GetHotelDto>()
                .ForMember(hotel => hotel.FeaturedImage, opt => opt.MapFrom(src => src.Galleries.FirstOrDefault(gallery => gallery.IsFeature).ImageUrl))
                .ForMember(hotel => hotel.Rating, opt => opt.MapFrom(src => src.Ratings.Count == 0 ? 0 : (double)src.Ratings.Sum(customer => customer.Ratings) / ((double)src.Ratings.Count)))
                .ForMember(hotel => hotel.NumberOfReviews, opt => opt.MapFrom(src => src.Reviews.Count))
                .ForMember(hotel => hotel.Gallery, opt => opt.MapFrom(src => src.Galleries.Select(gallery => gallery.ImageUrl).ToList()));

            CreateMap<Hotel, GetAllHotelDto>()
               .ForMember(hotel => hotel.FeaturedImage, opt => opt.MapFrom(src => src.Galleries.FirstOrDefault(gallery => gallery.IsFeature).ImageUrl))
               .ForMember(hotel => hotel.Rating, opt => opt.MapFrom(src => src.Ratings.Count == 0 ? 0 : (double)src.Ratings.Sum(customer => customer.Ratings) / ((double)src.Ratings.Count)))
               .ForMember(hotel => hotel.Gallery, opt => opt.MapFrom(src => src.Galleries.Select(gallery => gallery.ImageUrl).ToList()));

            CreateMap<Payment, TransactionsDto>().ReverseMap();


            // Room Maps
            CreateMap<Room, AddRoomDto>().ReverseMap();
            CreateMap<Room, AddRoomResponseDto>().ReverseMap();
            CreateMap<Room, RoomDTo>().ReverseMap();
            

            // RoomType Maps
            CreateMap<RoomType, RoomInfoDto>().ReverseMap();
            CreateMap<RoomType, RoomTypeByHotelDTo>();
            CreateMap<RoomType, RoomTypeDto>();
            CreateMap<RoomType, RoomTypeRequestDto>().ReverseMap();
            CreateMap<RoomType, RoomTypeDetailsDto>()
                .ForMember(x => x.HotelName, y => y.MapFrom(u => u.Hotel.Name))
                .ForMember(x => x.DateCreated, y => y.MapFrom(u => u.CreatedAt))
                .ForMember(x => x.HotelAddress, y => y.MapFrom(u => u.Hotel.Address))
                .ForMember(x => x.PricePerNight, y => y.MapFrom(u => u.Price))
                .ForMember(x => x.Thumbnail, y => y.MapFrom(u => u.Thumbnail))
                .ForMember(x => x.Name, y => y.MapFrom(u => u.Name));





            // Rating Maps
            CreateMap<Rating, HotelRatingsDTo>();
            CreateMap<Rating, AddRatingDto>().ReverseMap();

            // Gallery Maps
            CreateMap<Gallery, GalleryDto>().ReverseMap();

            //Customer
            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();
            CreateMap<Customer, CustomerDetailsToReturnDto>()
                .ForMember(x => x.FirstName, y => y.MapFrom(u => u.AppUser.FirstName))
                .ForMember(x => x.LastName, y => y.MapFrom(u => u.AppUser.LastName))
                .ForMember(x => x.Age, y => y.MapFrom(u => u.AppUser.Age))
                .ForMember(x => x.Id, y => y.MapFrom(u => u.AppUser.Id))
                .ForMember(x => x.Email, y => y.MapFrom(u => u.AppUser.Email))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(u => u.AppUser.PhoneNumber))
                .ForMember(x => x.UserName, y => y.MapFrom(u => u.AppUser.UserName))
                .ForMember(x => x.Age, y => y.MapFrom(u => u.AppUser.Age))
                .ForMember(x => x.CreditCard, y => y.MapFrom(u => u.CreditCard))
                .ForMember(x => x.Address, y => y.MapFrom(u => u.Address))
                .ForMember(x => x.State, y => y.MapFrom(u => u.State))
                .ForMember(x => x.Avatar, y => y.MapFrom(u => u.AppUser.Avatar));


            //TransactionResponse Mapper

            //Transaction Maps
            CreateMap<Booking, TransactionResponseDto>()
                .ForMember(x => x.BookingId, y => y.MapFrom(s => s.Id))
                 .ForMember(x => x.HotelName, y => y.MapFrom(s => s.Hotel.Name))
                 .ForMember(x => x.PaymentStatus, y => y.MapFrom(s => s.Payment.Status))
                 .ForMember(x => x.PaymentMethod, y => y.MapFrom(s => s.Payment.MethodOfPayment))
                 .ForMember(x => x.PaymentMethod, y => y.MapFrom(s => s.Payment.CreatedAt))
                 .ForMember(x => x.PaymentMethod, y => y.MapFrom(s => s.Payment.Amount))
                 .ForMember(x => x.PaymentMethod, y => y.MapFrom(s => s.Customer.AppUserId))
                 .ForMember(x => x.CustomerName, y => y.MapFrom(s => s.Customer.AppUser.FirstName + " " + s.Customer.AppUser.LastName));


            // aminity
            CreateMap<Amenity, AmenityDto>();

            // reviewdto
            CreateMap<Review, ReviewDto>()
                .ForMember(review => review.CustomerImage, opt => opt.MapFrom(review => review.Customer.AppUser.Avatar))
                .ForMember(review => review.Text, opt => opt.MapFrom(review => review.Comment))
                .ForMember(review => review.Date, opt => opt.MapFrom(review => review.CreatedAt.ToShortDateString()));

            CreateMap<Customer, GetUsersResponseDto>()
                .ForMember(x => x.FirstName, y => y.MapFrom(u => u.AppUser.FirstName))
                .ForMember(x => x.LastName, y => y.MapFrom(u => u.AppUser.LastName))
                .ForMember(x => x.Age, y => y.MapFrom(u => u.AppUser.Age))
                .ForMember(x => x.Id, y => y.MapFrom(u => u.AppUser.Id))
                .ForMember(x => x.Email, y => y.MapFrom(u => u.AppUser.Email))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(u => u.AppUser.PhoneNumber))
                .ForMember(x => x.UserName, y => y.MapFrom(u => u.AppUser.UserName))
                .ForMember(x => x.Age, y => y.MapFrom(u => u.AppUser.Age))
                .ForMember(x => x.State, y => y.MapFrom(u => u.State))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(u => u.AppUser.CreatedAt));

            //Review Maps
            CreateMap<Review, ReviewToReturnDto>()
                .ForMember(x => x.FirstName, x => x.MapFrom(x => x.Customer.AppUser.FirstName))
                .ForMember(x => x.LastName, x => x.MapFrom(x => x.Customer.AppUser.LastName))
                .ForMember(x => x.Avatar, x => x.MapFrom(x => x.Customer.AppUser.Avatar));

            CreateMap<Review, AddReviewDto>().ReverseMap();
            CreateMap<Review, AddReviewToReturnDto>().ReverseMap();

            // IWshList Maps
            CreateMap<WishList, CustomerWishListDto>()
                .ForMember(x => x.HotelName, y => y.MapFrom(c => c.Hotel.Name))
                .ForMember(x => x.ImageUrl, y => y.MapFrom(src => src.Hotel.Galleries.FirstOrDefault(opt => opt.IsFeature).ImageUrl));

            // Transaction Maps
            CreateMap<Booking, TransactionResponseDto>()
                .ForMember(x => x.BookingId, y => y.MapFrom(s => s.Id))
                 .ForMember(x => x.HotelName, y => y.MapFrom(s => s.Hotel.Name))
                 .ForMember(x => x.PaymentMethod, y => y.MapFrom(s => s.Payment.MethodOfPayment))
                 .ForMember(x => x.CustomerName, y => y.MapFrom(s => s.Customer.AppUser.FirstName + " " + s.Customer.AppUser.LastName));


            //Manager Maps
            CreateMap<Manager, ManagerDto>().ReverseMap();

            CreateMap<AppUser, ManagerDto>()
                .ForMember(manager => manager.BusinessEmail, u => u.MapFrom(user => user.Email))
                .ForMember(manager => manager.BusinessEmail, u => u.MapFrom(user => user.UserName))
                .ForMember(manager => manager.BusinessPhone, u => u.MapFrom(user => user.PhoneNumber))
                .ReverseMap();

            CreateMap<Hotel, ManagerDto>()
                .ForMember(hotel => hotel.HotelAddress, u => u.MapFrom(hotel => hotel.Address))
                .ForMember(hotel => hotel.HotelCity, u => u.MapFrom(hotel => hotel.City))
                .ForMember(hotel => hotel.HotelDescription, u => u.MapFrom(hotel => hotel.Description))
                .ForMember(hotel => hotel.HotelEmail, u => u.MapFrom(hotel => hotel.Email))
                .ForMember(hotel => hotel.HotelName, u => u.MapFrom(hotel => hotel.Name))
                .ForMember(hotel => hotel.HotelPhone, u => u.MapFrom(hotel => hotel.Phone))
                .ForMember(hotel => hotel.HotelState, u => u.MapFrom(hotel => hotel.State))
                .ReverseMap();

            CreateMap<Manager, ManagerDetailsResponseDto>()
                .ForMember(x => x.FirstName, o => o.MapFrom(y => y.AppUser.FirstName))
                .ForMember(x => x.LastName, o => o.MapFrom(y => y.AppUser.LastName))
                .ForMember(x => x.ManagerPhone, o => o.MapFrom(y => y.AppUser.PhoneNumber))
                .ForMember(x => x.Age, o => o.MapFrom(y => y.AppUser.Age))
                .ForMember(x => x.CompanyName, o => o.MapFrom(y => y.CompanyName))
                .ForMember(x => x.BusinessEmail, o => o.MapFrom(y => y.BusinessEmail))
                .ForMember(x => x.BusinessPhone, o => o.MapFrom(y => y.BusinessPhone))
                .ForMember(x => x.CompanyAddress, o => o.MapFrom(y => y.CompanyAddress))
                .ForMember(x => x.AccountName, o => o.MapFrom(y => y.AccountName))
                .ForMember(x => x.State, o => o.MapFrom(y => y.State))
                .ForMember(x => x.AccountNumber, o => o.MapFrom(y => y.AccountNumber))
                .ReverseMap();


            //AppUser Maps
            CreateMap<Customer, TopManagerCustomers>()
                .ForMember(d => d.FirstName, o => o.MapFrom(u => u.AppUser.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(u => u.AppUser.LastName))
                .ForMember(d => d.Gender, o => o.MapFrom(u => u.AppUser.Gender))
                .ForMember(d => d.Email, o => o.MapFrom(u => u.AppUser.Email))
                .ForMember(d => d.Avatar, o => o.MapFrom(u => u.AppUser.Avatar))
                .ForMember(d => d.CustomerId, o => o.MapFrom(u => u.AppUserId))
                .ForMember(d => d.NumberOfBookedTimes, o => o.MapFrom(u => u.Bookings.Count))
                .ForMember(d => d.TotalAmountSpent, o => o.MapFrom(u => u.Bookings.Sum(bk => bk.Payment.Amount)))
                .ReverseMap();
            //Manager Request Map

            CreateMap<ManagerRequest, ManagerRequestDto>().ReverseMap();
            CreateMap<ManagerRequest, ManagerRequestResponseDTo>();


            CreateMap<Manager, HotelManagersDto>()
                .ForMember(x => x.ManagerId, y => y.MapFrom(z => z.AppUser.Id))
                .ForMember(x => x.Age, y => y.MapFrom(z => z.AppUser.Age))
                .ForMember(x => x.AccountName, y => y.MapFrom(z => z.AccountName))
                .ForMember(x => x.AccountNumber, y => y.MapFrom(z => z.AccountNumber))
                .ForMember(x => x.Avatar, y => y.MapFrom(z => z.AppUser.Avatar))
                .ForMember(x => x.BusinessEmail, y => y.MapFrom(z => z.BusinessEmail))
                .ForMember(x => x.BusinessPhone, y => y.MapFrom(z => z.BusinessPhone))
                .ForMember(x => x.CompanyAddress, y => y.MapFrom(z => z.CompanyAddress))
                .ForMember(x => x.CompanyName, y => y.MapFrom(z => z.CompanyName))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => z.AppUser.CreatedAt))
                .ForMember(x => x.UpdatedAt, y => y.MapFrom(z => z.AppUser.UpdatedAt))
                .ForMember(x => x.FirstName, y => y.MapFrom(z => z.AppUser.FirstName))
                .ForMember(x => x.LastName, y => y.MapFrom(z => z.AppUser.LastName))
                .ForMember(x => x.State, y => y.MapFrom(z => z.State))
                .ForMember(x => x.IsActive, y => y.MapFrom(z => z.AppUser.IsActive))
                .ForMember(x => x.Gender, y => y.MapFrom(z => z.AppUser.Gender))
                .ForMember(x => x.TotalHotels, y => y.MapFrom(z => z.Hotels.Count))
                .ForMember(x => x.TotalAmount,
                    y => y.MapFrom(x => x.Hotels.Select(x => x.Bookings.Select(x => x.Payment.Amount).Sum()).Sum()))
                .ForMember(x => x.HotelNames, y => y.MapFrom(z => z.Hotels.Select(q => q.Name)))
                .ForMember(x => x.HotelStreet, y => y.MapFrom(z => z.Hotels.Select(q => q.Address)))
                .ForMember(x => x.HotelCity, y => y.MapFrom(z => z.Hotels.Select(q => q.City)))
                .ForMember(x => x.HotelState, y => y.MapFrom(z => z.Hotels.Select(q => q.State)))
                .ForMember(x => x.ManagerEmail, y => y.MapFrom(z => z.AppUser.Email))
                .ForMember(x => x.ManagerPhone, y => y.MapFrom(z => z.AppUser.PhoneNumber))
                ;

        }
    }
}
