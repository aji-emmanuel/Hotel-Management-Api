using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<Room> _dbSet;
        public RoomRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Room>();
        }

        public async Task<List<Room>> GetAllAsync(Expression<Func<Room, bool>> expression = null, Func<IQueryable<Room>, IOrderedQueryable<Room>> orderby = null, List<string> Includes = null)
        {
            var query = _dbSet.AsNoTracking();
            if (Includes != null) Includes.ForEach(x => query = query.Include(x));
            if (expression != null) query = query.Where(expression);
            if (orderby != null) query = orderby(query);
            return await query.ToListAsync();
        }

        public IQueryable<RoomType> GetRoomTypeByHotel(string hotelId)
        {
            var rooms = _context.RoomTypes
                .Where(room => room.Hotel.Id == hotelId);

            return rooms;
        }

        public async Task<ICollection<Room>> GetHotelRoom(string hotelId, string roomTypeId)
        {
            var getRoom = await _context.RoomTypes
                .Include(x => x.Rooms)
                .Where(x => x.Id == roomTypeId).Where(x => x.HotelId == hotelId)
                .Select(x => x.Rooms).FirstOrDefaultAsync();

            return getRoom;
        }

        public async Task<Room> GetRoomByIdAsync(string roomId)
        {
            return await _context.Rooms.Include(x => x.Roomtype)
                .ThenInclude(x => x.Hotel).FirstOrDefaultAsync(x => x.Id == roomId);
        }

        

    }
}
