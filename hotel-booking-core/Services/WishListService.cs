using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_models;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class WishListService : IWishListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public WishListService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Response<string>> AddToWishList(string hotelId, string customerId)
        {
            _logger.Information("Initiating AddWishList");
            var hotel = await _unitOfWork.Hotels.GetHotelById(hotelId);
            if(hotel == null)
            {
                _logger.Error("Hotel not found");
                return Response<string>.Fail("Hotel not found");
            }

            _logger.Information("Checking if Hotel already exists in WishList");
            var wishlist = await _unitOfWork.WishLists.CheckWishListAsync(customerId, hotelId);

            if(wishlist == null)
            {
                _logger.Information("Creating new Wishlist");
                wishlist = new WishList()
                {
                    CustomerId = customerId,
                    HotelId = hotelId
                };
                await _unitOfWork.WishLists.InsertAsync(wishlist);
                await _unitOfWork.Save();
                _logger.Information("Hotel added to Wishlist successfully");
                return Response<string>.Success("WishList Added", "WishList Added", StatusCodes.Status201Created);
            }
            _logger.Error("Hotel already exists in wishlist");
            return Response<string>.Fail("WishList already existing", StatusCodes.Status409Conflict);            
        }

        public async Task<Response<string>> ClearWishList(string customerId)
        {
            _logger.Information("Getting customer wishlist");
            var wishLists = _unitOfWork.WishLists.GetCustomerWishList(customerId);
            if(wishLists.Count() >= 1)
            {
                _logger.Information("Initiating Clearing Wishlist");
                _unitOfWork.WishLists.DeleteRange(wishLists);
                await _unitOfWork.Save();
                _logger.Information("Wishlist succesfully cleared");
                return Response<string>.Success("Wishlist Cleared", "Wishlist Cleared", StatusCodes.Status200OK);
            }
            _logger.Error("Wishlist is empty");
            return Response<string>.Fail("Wishlist empty");
        }

        public async Task<Response<string>> RemoveFromWishList(string hotelId, string customerId)
        {
            _logger.Information("Getting customer wishlist by customer Id and hotel Id");
            var wishlist = await _unitOfWork.WishLists.CheckWishListAsync(customerId, hotelId);
            if(wishlist != null)
            {
                _logger.Information("Initiating remove hotel from customer wishlist");
                _unitOfWork.WishLists.Delete(wishlist);
                await _unitOfWork.Save();
                _logger.Information("Hotel successfully removed from customer wishlist");
                return Response<string>.Success("WishList Removed", "WishList Removed", StatusCodes.Status200OK);
            }
            _logger.Error("Hotel not found in customer wishlist");
            return Response<string>.Fail("Wishlist not found");
        }
    }
}
