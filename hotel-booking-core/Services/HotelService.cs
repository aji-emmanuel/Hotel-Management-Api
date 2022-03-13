using AutoMapper;
using hotel_booking_core.Interface;
using hotel_booking_core.Interfaces;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.RatingDtos;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_dto.RoomDtos;
using hotel_booking_models;
using hotel_booking_models.Cloudinary;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IImageService _imageService;

        public HotelService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger,
            IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _imageService = imageService;
        }

        public async Task<Response<UpdateImageDto>> UpdateHotelImage(AddHotelAndRoomTypeImageDto model, string hotelId)
        {
            var hotel = await _unitOfWork.Hotels.GetHotelById(hotelId);

            if (hotel != null)
            {
                var upload = await _imageService.UploadImageForHotelAsync(model.File);

                string url = upload.SecureUrl.ToString();

                if (!hotel.Galleries.Any())
                {
                    var gallery = new Gallery
                    {
                        ImageUrl = url,
                        IsFeature = true
                    };
                    hotel.Galleries.Add(gallery);

                    _unitOfWork.Hotels.Update(hotel);
                    await _unitOfWork.Save();

                    return Response<UpdateImageDto>.Success("image upload successful", new UpdateImageDto { Url = url });
                }
                var getHotelGallery = hotel.Galleries.FirstOrDefault();
                getHotelGallery.ImageUrl = url;
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();

                return Response<UpdateImageDto>.Success("image upload successful", new UpdateImageDto { Url = url });
            }
            return Response<UpdateImageDto>.Fail("hotel not found");
        }

        public async Task<Response<UpdateImageDto>> UpdateRoomTypeImage(AddImageDto imageDto, string roomTypeId)
        {
            var roomType = await _unitOfWork.Hotels.GetHotelRoomTypeById(roomTypeId);

            if (roomType != null)
            {
                var upload = await _imageService.UploadAsync(imageDto.Image);
                string url = upload.Url.ToString();
                roomType.Thumbnail = url;
                _unitOfWork.RoomType.Update(roomType);
                await _unitOfWork.Save();

                return Response<UpdateImageDto>.Success("image upload successful", new UpdateImageDto { Url = url });
            }
            return Response<UpdateImageDto>.Fail("roomType not found");
        }

        public async Task<Response<string>> AddRoomTypeToHotel(string hotelId, RoomTypeRequestDto model)
        {
            model.Name = model.Name[0].ToString().ToUpper() + model.Name.Substring(1).ToLower();
            var hotel = await _unitOfWork.Hotels.GetHotelById(hotelId);
            var response = new Response<string>();
            var roomType = _mapper.Map<RoomType>(model);
            var check = _unitOfWork.Hotels.GetHotelWithRoomTypes(hotelId, model);


            if (hotel != null)
            {
                if (check)
                {
                    roomType.HotelId = hotelId;
                    await _unitOfWork.RoomType.InsertAsync(roomType);
                    await _unitOfWork.Save();

                    response.Succeeded = true;
                    response.Message = $"RoomType added to hotel successfully";
                    response.StatusCode = (int)HttpStatusCode.OK;
                    return response;
                }
                response.Succeeded = false;
                response.Message = $"A room type already exists with the given name";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }
            response.Succeeded = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = "Hotel does not exist";
            return response;

        }

        public async Task<Response<IEnumerable<HotelCustomersDto>>> GetCustomersByHotelId(string hotelId)
        {
            var bookings = await _unitOfWork.Hotels.GetCustomersByHotelId(hotelId);
            var customers = _mapper.Map<IEnumerable<HotelCustomersDto>>(bookings);
            return new Response<IEnumerable<HotelCustomersDto>>(StatusCodes.Status200OK, true, "Customers of this hotel are as follows", customers);

        }

        public async Task<Response<IEnumerable<HotelBasicDetailsDto>>> GetHotelsByRatingsAsync()
        {
            var hotelList = await _unitOfWork.Hotels.GetHotelsByRating().ToListAsync();
            var hotelListDto = _mapper.Map<IEnumerable<HotelBasicDetailsDto>>(hotelList);
            var response = new Response<IEnumerable<HotelBasicDetailsDto>>(StatusCodes.Status200OK, true, "hotels by ratings", hotelListDto);
            return response;
        }

        public async Task<Response<PageResult<IEnumerable<RoomInfoDto>>>> GetRoomByPriceAsync(PriceDto priceDto)
        {
            var roomQuery = _unitOfWork.RoomType.GetRoomByPrice(priceDto.MinPrice, priceDto.MaxPrice);
            var pageResult = await roomQuery.PaginationAsync<RoomType, RoomInfoDto>(priceDto.PageSize, priceDto.PageNumber, _mapper);
            var response = new Response<PageResult<IEnumerable<RoomInfoDto>>>(StatusCodes.Status200OK, true, "List of rooms by price", pageResult);
            return response;
        }

        public async Task<Response<IEnumerable<HotelBasicDetailsDto>>> GetTopDealsAsync()
        {
            var hotelList = await _unitOfWork.Hotels.GetTopDeals().ToListAsync(); ;
            var hotelListDto = _mapper.Map<IEnumerable<HotelBasicDetailsDto>>(hotelList);
            var response = new Response<IEnumerable<HotelBasicDetailsDto>>(StatusCodes.Status200OK, true, "hotels top deals", hotelListDto);
            return response;
        }

        public async Task<Response<PageResult<IEnumerable<GetAllHotelDto>>>> GetAllHotelsAsync(PagingDto paging)
        {
            var hotelQueryable = _unitOfWork.Hotels.GetAllHotels();
            var hotelList = await hotelQueryable.PaginationAsync<Hotel, GetAllHotelDto>(paging.PageSize, paging.PageNumber, _mapper);
            var response = new Response<PageResult<IEnumerable<GetAllHotelDto>>>(StatusCodes.Status200OK, true, "List of all hotels", hotelList);
            return response;
        }
        public async Task<Response<IEnumerable<RoomDTo>>> GetHotelRooomById(string hotelId, string roomTypeId)
        {
            var room = await _unitOfWork.Rooms.GetHotelRoom(hotelId, roomTypeId);


            if (room != null)
            {
                var response = _mapper.Map<IEnumerable<RoomDTo>>(room);

                var result = new Response<IEnumerable<RoomDTo>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = $"Hotel Rooms for roomType with id {roomTypeId} in hotel with  id {hotelId}",
                    Data = response
                };
                return result;
            }
            return Response<IEnumerable<RoomDTo>>.Fail("No room found for this particular roomtype", StatusCodes.Status404NotFound);
        }

        public async Task<Response<PageResult<IEnumerable<RoomTypeByHotelDTo>>>> GetHotelRoomType(PagingDto paging, string hotelId)
        {
            var roomList = _unitOfWork.Rooms.GetRoomTypeByHotel(hotelId);

            if (roomList.Any())
            {
                var item = await roomList.PaginationAsync<RoomType, RoomTypeByHotelDTo>(paging.PageSize, paging.PageNumber, _mapper);
                var result = new Response<PageResult<IEnumerable<RoomTypeByHotelDTo>>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = $"Total RoomType in hotel with id {hotelId}",
                    Data = item
                };
                return result;
            }
            return Response<PageResult<IEnumerable<RoomTypeByHotelDTo>>>.Fail("Hotel is not valid", StatusCodes.Status404NotFound);
        }

        public async Task<Response<IEnumerable<HotelRatingsDTo>>> GetHotelRatings(string hotelId)
        {
            var ratings = await _unitOfWork.Hotels.HotelRatings(hotelId);

            if (ratings.Any())
            {
                var response = _mapper.Map<IEnumerable<HotelRatingsDTo>>(ratings);

                var result = new Response<IEnumerable<HotelRatingsDTo>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = $"cummulated ratings for hotel with id {hotelId}",
                    Data = response
                };
                return result;
            }
            return Response<IEnumerable<HotelRatingsDTo>>.Fail("No ratings for this hotel", StatusCodes.Status404NotFound);
        }

        public async Task<Response<GetHotelDto>> GetHotelByIdAsync(string id)
        {
            var response = new Response<GetHotelDto>();
            Hotel hotel = await _unitOfWork.Hotels.GetHotelEntitiesById(id);
            if (hotel != null)
            {
                GetHotelDto hotelDto = _mapper.Map<GetHotelDto>(hotel);

                response.Data = hotelDto;
                response.Succeeded = true;
                response.Message = $"Details for Hotel with Id: {id}";
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
            }
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Succeeded = false;
            response.Data = default;
            response.Message = $"Hotel with Id: {id} not found";
            return response;
        }

        public async Task<Response<UpdateHotelDto>> UpdateHotelAsync(string hotelId, UpdateHotelDto model)
        {
            var response = new Response<UpdateHotelDto>();
            // Get the hotel to be updated using it's Id
            Hotel hotel = await _unitOfWork.Hotels.GetHotelEntitiesById(hotelId);
            if (hotel != null)
            {
                hotel.Name = model.Name;
                hotel.Description = model.Description;
                hotel.Email = model.Email;
                hotel.Phone = model.PhoneNumber;
                hotel.Address = model.Address;
                hotel.City = model.City;
                hotel.State = model.State;
                hotel.UpdatedAt = DateTime.Now;

                // Update the hotel and save changes to database
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();

                // Map properties of updated hotel to the response DTO
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Succeeded = true;
                response.Message = $"Hotel with id {hotel.Id} has been updated";
                response.Data = model;
                return response;
            }
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Hotel with id {hotelId} was not found!";
            response.Succeeded = false;
            return response;
        }

        public async Task<Response<AddHotelResponseDto>> AddHotel(string managerId, AddHotelDto hotelDto)
        {
            Hotel hotel = _mapper.Map<Hotel>(hotelDto);

            hotel.CreatedAt = DateTime.Now;

            hotel.ManagerId = managerId;

            await _unitOfWork.Hotels.InsertAsync(hotel);
            await _unitOfWork.Save();

            var hotelResponse = _mapper.Map<AddHotelResponseDto>(hotel);

            var response = new Response<AddHotelResponseDto>()
            {
                StatusCode = StatusCodes.Status200OK,
                Succeeded = true,
                Data = hotelResponse,
                Message = $"{hotel.Name} with id {hotel.Id} has been added"
            };
            return response;
        }

        public async Task<Response<AddRoomResponseDto>> AddHotelRoom(string hotelId, AddRoomDto roomDto)
        {

            var response = new Response<AddRoomResponseDto>();
            var checkHotelId = await _unitOfWork.Hotels.GetHotelEntitiesById(hotelId);
            if (checkHotelId == null)
            {
                response.Succeeded = false;
                response.Data = null;
                response.Message = "Hotel Not Found";
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }

            var checkRoomType = await _unitOfWork.RoomType.CheckForRoomTypeAsync(roomDto.RoomTypeId);

            if (checkRoomType == null)
            {
                response.Succeeded = false;
                response.Data = null;
                response.Message = "Roomtype not on the list";
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }
            var room = _mapper.Map<Room>(roomDto);
            await _unitOfWork.Rooms.InsertAsync(room);
            await _unitOfWork.Save();
            var roomResponse = _mapper.Map<AddRoomResponseDto>(room);


            response.Succeeded = true;
            response.Data = roomResponse;
            response.Message = $"Room with id {room.Id} added to Hotel with id {hotelId}";
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
        }

        public async Task<Response<PageResult<IEnumerable<TransactionsDto>>>> GetHotelTransaction(string hotelId, PagingDto paging)
        {
            var hotel = await _unitOfWork.Hotels.GetHotelById(hotelId);
            if (hotel != null)
            {
                var transactionsQueryable = _unitOfWork.Payments.GetHotelTransactions(hotelId);
                var pageResult = await transactionsQueryable.PaginationAsync<Payment, TransactionsDto>(paging.PageSize, paging.PageNumber, _mapper);
                return new Response<PageResult<IEnumerable<TransactionsDto>>>(StatusCodes.Status200OK, true, "hotel transactions", pageResult);
            }
            return Response<PageResult<IEnumerable<TransactionsDto>>>.Fail("Hotel Not Found");
        }
        public async Task<Response<string>> DeleteHotelByIdAsync(string hotelId)
        {
            var hotel = await _unitOfWork.Hotels.GetHotelById(hotelId);
            var response = new Response<string>();

            if (hotel != null)
            {
                _unitOfWork.Hotels.Delete(hotel);
                await _unitOfWork.Save();

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = $"Hotel with Id = {hotelId} has been deleted";
                response.Data = default;
                response.Succeeded = true;
                return response;
            }
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"Hotel with id = {hotelId} does not exist";
            response.Succeeded = false;
            return response;
        }
        public async Task<Response<PageResult<IEnumerable<GetAllHotelDto>>>> GetHotelByLocation(string location, PagingDto paging)
        {
            _logger.Information($"Attempting to get hotel in {location}");
            var hotels = _unitOfWork.Hotels.GetAllHotels()
                .Where(q => q.State.ToLower().Contains(location.ToLower()) || q.City.ToLower().Contains(location.ToLower()));

            var response = new Response<PageResult<IEnumerable<GetAllHotelDto>>>();

            if (hotels != null)
            {
                _logger.Information("Search completed successfully");
                var result = await hotels.PaginationAsync<Hotel, GetAllHotelDto>
                    (
                        pageSize: paging.PageSize,
                        pageNumber: paging.PageNumber,
                        mapper: _mapper
                    );

                response.Data = result;
                response.StatusCode = StatusCodes.Status200OK;
                response.Succeeded = true;
                return response;
            }

            _logger.Information("Search completed with no results");
            response.Data = default;
            response.StatusCode = StatusCodes.Status200OK;
            response.Message = "On your request nothing has been found.";
            response.Succeeded = false;
            return response;
        }

        public async Task<Response<Dictionary<string, int>>> GetNumberOfHotelsPerLocation()
        {
            _logger.Information($"Attempting to get all hotels");
            var hotels = await _unitOfWork.Hotels.GetAll().ToListAsync();
            var result = new Dictionary<string, int>();

            foreach (var hotel in hotels)
            {
                if (!result.ContainsKey(hotel.State))
                {
                    result.Add(hotel.State, 1);
                }
                else
                {
                    result[hotel.State] += 1;
                }
            }

            var response = new Response<Dictionary<string, int>>();

            _logger.Information("Search completed successfully");

            response.Data = result;
            response.StatusCode = StatusCodes.Status200OK;
            response.Succeeded = true;
            return response;
        }


        public async Task<Response<PageResult<IEnumerable<ReviewToReturnDto>>>> GetAllReviewsByHotelAsync(PagingDto paging, string hotelId)
        {
            _logger.Information($"Attemp to get all review by hotel id {hotelId}");
            var response = new Response<PageResult<IEnumerable<ReviewToReturnDto>>>();
            var hotelExistCheck = await _unitOfWork.Hotels.GetHotelById(hotelId);

            if (hotelExistCheck == null)
            {
                _logger.Information("Get all reviews by hotelId failed");
                response.Succeeded = false;
                response.Data = null;
                response.Message = "Hotel does not exist";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }

            var reviews = _unitOfWork.Reviews.GetAllReviewsByHotelAsync(hotelId);

            //_mapper.Map<Review>(reviews);

            var pageResult = await reviews.PaginationAsync<Review, ReviewToReturnDto>(paging.PageSize, paging.PageNumber, _mapper);
            _logger.Information("Get all reviews operation successful");
            response.Succeeded = true;
            response.Data = pageResult;
            response.Message = $"List of all reviews in hotel with id {hotelId}";
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
        }

        public async Task<Response<IEnumerable<TopCustomerDto>>> TopHotelCustomers(string hotelId)
        {
            var hotelBookings = await _unitOfWork.Booking.GetBookingsByHotelId(hotelId).ToListAsync();

            var customers = hotelBookings.GroupBy(x => x.CustomerId).Select(x =>
           new CustomerBookingSum
           {
               CustomerId = x.Key,
               Amount = x.Select(x => x.Payment.Amount).Sum(),
               Customer = x.Select(x => x.Customer).First()
           }).Select(x => x.Customer).Take(5).ToList();
            var pageResult = _mapper.Map<IEnumerable<Customer>, IEnumerable<TopCustomerDto>>(customers);
            Response<IEnumerable<TopCustomerDto>> response = new()
            {
                Data = pageResult,
                Message = "Customer Bookings Fetched",
                StatusCode = StatusCodes.Status200OK,
                Succeeded = true
            };
            return response;
        }

        public async Task<Response<string>> RateHotel(string hotelId, string customerId, AddRatingDto ratingDto)
        {
            _logger.Information($"Attempt to rate hotel by customer id {customerId}");
            var rating = _mapper.Map<Rating>(ratingDto);
            rating.HotelId = hotelId;
            rating.CustomerId = customerId;

            var confirmHotel = await _unitOfWork.Hotels.GetHotelById(rating.HotelId);
            if (confirmHotel == null)
            {
                _logger.Information($"Rating hotel failed, hotel with id {hotelId} does not exist");
                return Response<string>.Fail($"Hotel with hotel id {rating.HotelId} not exist.");
            }

            var prevRating = await _unitOfWork.Rating.GetRatingsByHotel(hotelId, customerId);
            if (prevRating != null)
            {
                prevRating.Ratings = ratingDto.Ratings;
                prevRating.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.Rating.Update(prevRating);
                await _unitOfWork.Save();
                return Response<string>.Success("Rating updated successfully", prevRating.HotelId);
            }

            await _unitOfWork.Rating.InsertAsync(rating);
            await _unitOfWork.Save();

            _logger.Information("Rating hotel successful");
            var response = Response<string>.Success($"Rating added successfully", rating.HotelId);

            return response;
        }


        public async Task<Response<Room>> DeleteHotelRoomByIdAsync(string roomId)
        {
            var room = await _unitOfWork.Rooms.GetRoomByIdAsync(roomId);

            if (room != null)
            {
                _unitOfWork.Rooms.Delete(room);
                await _unitOfWork.Save();
                return new Response<Room>(StatusCodes.Status204NoContent, true, $"Room With the Id:{roomId} deleted sucessfully", null);
            }

            return new Response<Room>(StatusCodes.Status404NotFound, false, $"No Room With the Id:{roomId} not found", null);
        }

        public async Task<Response<RoomTypeDetailsDto>> GetRoomTypeDetails(string roomTypeId)
        {
            var response = new Response<RoomTypeDetailsDto>();
            if (roomTypeId == null)
            {
                response.Succeeded = false;
                response.Data = null;
                response.Message = "Invalid input";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }
            var roomType = await _unitOfWork.RoomType.GetRoomTypeDetails(roomTypeId);
            if (roomType == null)
            {
                response.Succeeded = false;
                response.Data = null;
                response.Message = "Not found";
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }
            var result = _mapper.Map<RoomTypeDetailsDto>(roomType);
            response.Succeeded = true;
            response.Data = result;
            response.Message = $"Room type details fetch successfully";
            response.StatusCode = (int)HttpStatusCode.OK;

            return response;
        }
    }
}
