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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IHotelService
    {
        /// <summary>
        /// Fetches and hotel using it's Id. Returns the hotel object and it's child entities
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<GetHotelDto>> GetHotelByIdAsync(string id);
        /// <summary>
        /// Updates an hotel asynchronously and returns update hotel response
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<UpdateHotelDto>> UpdateHotelAsync(string hotelId, UpdateHotelDto model);
        Task<Response<PageResult<IEnumerable<RoomTypeByHotelDTo>>>> GetHotelRoomType(PagingDto paging, string hotelId);
        Task<Response<IEnumerable<HotelRatingsDTo>>> GetHotelRatings(string hotelId);
        Task<Response<IEnumerable<RoomDTo>>> GetHotelRooomById(string hotelId, string roomTypeId);
        Task<Response<AddHotelResponseDto>> AddHotel(string managerId, AddHotelDto hotelDto);
        Task<Response<AddRoomResponseDto>> AddHotelRoom(string hotelid, AddRoomDto roomDto);
        Task<Response<string>> DeleteHotelByIdAsync(string hotelId);
        Task<Response<IEnumerable<HotelBasicDetailsDto>>> GetHotelsByRatingsAsync();
        Task<Response<PageResult<IEnumerable<RoomInfoDto>>>> GetRoomByPriceAsync(PriceDto priceDto);
        Task<Response<IEnumerable<HotelBasicDetailsDto>>> GetTopDealsAsync();

        /// <summary>
        /// Searches for hotels that are within the provided location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="paginator"></param>
        /// <returns>Returns an IEnumerable of hotels within the input location. Returns an empty array is search doesn't match any location in records</returns>
        Task<Response<PageResult<IEnumerable<GetAllHotelDto>>>> GetHotelByLocation(string location, PagingDto paging);
        
        Task<Response<Dictionary<string, int>>> GetNumberOfHotelsPerLocation();
       
        Task<Response<PageResult<IEnumerable<GetAllHotelDto>>>> GetAllHotelsAsync(PagingDto paging);
        
        Task<Response<PageResult<IEnumerable<ReviewToReturnDto>>>> GetAllReviewsByHotelAsync(PagingDto paging,
            string hotelId);

        Task<Response<IEnumerable<TopCustomerDto>>> TopHotelCustomers(string hotelId);

        Task<Response<string>> RateHotel(string hotelId, string customerId, AddRatingDto ratingDto);
       
        Task<Response<PageResult<IEnumerable<TransactionsDto>>>> GetHotelTransaction(string hotelId, PagingDto paging);

        Task<Response<Room>> DeleteHotelRoomByIdAsync(string roomId);

        Task<Response<string>> AddRoomTypeToHotel (string hotelId, RoomTypeRequestDto model);
        
        Task<Response<IEnumerable<HotelCustomersDto>>> GetCustomersByHotelId (string hotelId);      

        Task<Response<UpdateImageDto>> UpdateHotelImage(AddHotelAndRoomTypeImageDto model, string hotelId);
        Task<Response<UpdateImageDto>> UpdateRoomTypeImage(AddImageDto imageDto, string roomTypeId);

        Task<Response<RoomTypeDetailsDto>> GetRoomTypeDetails(string roomTypeId);
    }
}
