using hotel_booking_dto;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IWishListService
    {
        Task<Response<string>> AddToWishList(string hotelId, string customerId);
        Task<Response<string>> RemoveFromWishList(string hotelId, string customerId);
        Task<Response<string>> ClearWishList(string customerId);
    }
}
