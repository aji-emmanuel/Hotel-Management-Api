using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IRoomTypeRepository : IGenericRepository<RoomType>
    {
        IQueryable<RoomType> GetRoomByPrice(decimal minPrice, decimal maxPrice);
        Task<List<RoomType>> GetRoomTypesInEachHotel(string hotelId);
        IQueryable<RoomType> GetTopDeals();
        Task<RoomType> CheckForRoomTypeAsync(string roomTypeId);
        Task<RoomType> GetRoomTypeDetails(string roomTypeId);
    }
}
