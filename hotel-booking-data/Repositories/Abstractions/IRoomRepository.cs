using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<List<Room>> GetAllAsync(Expression<Func<Room, bool>> expression = null, Func<IQueryable<Room>, IOrderedQueryable<Room>> orderby = null, List<string> Includes = null);
        IQueryable<RoomType> GetRoomTypeByHotel(string hotelId);
        Task<ICollection<Room>> GetHotelRoom(string hotelId, string roomTypeId);
        Task<Room> GetRoomByIdAsync(string roomId);
    }
}
