using hotel_booking_models;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IWishListRepository : IGenericRepository<WishList>
    {
        IQueryable<WishList> GetCustomerWishList(string customerId);
        Task<WishList> CheckWishListAsync(string customerId, string hotelId);
    }
}
