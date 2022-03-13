using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class RoomTypeRepository : GenericRepository<RoomType>, IRoomTypeRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<RoomType> _dbSet;

        public RoomTypeRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<RoomType>();
        }


        public IQueryable<RoomType> GetRoomByPrice(decimal minPrice, decimal maxPrice)
        {
            var query = _dbSet.AsNoTracking();
            query = query.Include(rt => rt.Hotel);
            query = query.Where(rt => (!(maxPrice > minPrice) ? rt.Price >= minPrice
                                        : (rt.Price >= minPrice) && (rt.Price <= maxPrice)));
            query = query.OrderBy(rt => rt.Price);
            return query;
        }
        public IQueryable<RoomType> GetTopDeals()
        {
            var query = _dbSet.AsNoTracking();
            query = query.Include(rt => rt.Hotel).ThenInclude(y => y.Galleries);
            query = query.OrderBy(rt => rt.Price);
            return query;
        }

        public async Task<List<RoomType>> GetRoomTypesInEachHotel(string hotelId)
        {
            var rooms = await _context.RoomTypes.Where(x => x.HotelId == hotelId).ToListAsync();
            return rooms;
        }

        public async Task<RoomType> CheckForRoomTypeAsync(string roomTypeId)
        {
            var roomType = await _dbSet.AsNoTracking().Where(x => x.Id == roomTypeId).FirstOrDefaultAsync();
            return roomType;
        }

        public async Task<RoomType> GetRoomTypeDetails(string roomTypeId)
        {
            return await _dbSet.Where(x => x.Id == roomTypeId)
                .Include(x => x.Rooms)
                .ThenInclude(x=>x.Roomtype)
                .Include(x => x.Hotel)
                .ThenInclude(x=>x.Ratings).FirstOrDefaultAsync();
        }
    }
}