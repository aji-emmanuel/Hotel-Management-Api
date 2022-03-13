using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto.HotelDtos;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<Hotel> _dbSet;
        public HotelRepository (HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Hotel>();
        }
        public IQueryable<Hotel> GetHotelsByRating ()
        {
            var query = _dbSet.AsNoTracking();
            query = query.Include(x => x.Galleries)
                .Include(x => x.Ratings)
                .Include(x => x.RoomTypes)
                .OrderByDescending(h => h.Ratings.Count == 0 ? 0 : h.Ratings.Sum(r => r.Ratings) / (double)h.Ratings.Count)
                .Take(5);
            return query;
        }
        public IQueryable<Hotel> GetTopDeals ()
        {
            var query = _dbSet.AsNoTracking();
            query = query.Include(x => x.Galleries)
                .Include(x => x.Ratings)
                .Include(x => x.RoomTypes)
                .OrderBy(x => x.RoomTypes.OrderBy(rt => rt.Price).FirstOrDefault().Price)
                .Take(5);
            return query;
        }
        public IQueryable<Hotel> GetAllHotels ()
        {
            var hotelList = _dbSet.AsNoTracking()
               .Include(c => c.Galleries)
               .Include(c => c.Ratings)
               .Include(c => c.RoomTypes);
            return hotelList;
        }

        public IQueryable<Hotel> GetAll ()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Hotel> GetHotelEntitiesById (string hotelId)
        {
            var hotel = _dbSet.AsNoTracking()
                .Where(hotel => hotel.Id == hotelId)
                .Include(hotel => hotel.Galleries)
                .Include(hotel => hotel.Ratings)
                .Include(hotel => hotel.RoomTypes)
                .Include(hotel => hotel.Amenities)
                .Include(hotel => hotel.Reviews)
                .ThenInclude(review => review.Customer.AppUser);
            return await hotel.FirstOrDefaultAsync();
        }

        public async Task<List<Rating>> HotelRatings (string hotelId)
        {
            var ratings = await _context.Ratings
                    .Where(x => x.HotelId == hotelId).ToListAsync();

            return ratings;
        }

        public async Task<RoomType> GetHotelRoomTypeById(string roomTypeId)
        {
            var hotelRoomTypes = await _context.RoomTypes.FirstOrDefaultAsync(x => x.Id == roomTypeId);
            return hotelRoomTypes;
        }

        public bool GetHotelWithRoomTypes (string hotelId, RoomTypeRequestDto model)
        {
            var hotelRoomTypes = _context.RoomTypes.Where(x => x.HotelId == hotelId).ToList();
            var check = hotelRoomTypes.FirstOrDefault(x => x.Name == model.Name);
            return check == null;
        }

        public async Task<List<Booking>> GetCustomersByHotelId (string hotelId)
        {
            return await _context.Bookings.Where(x => x.HotelId == hotelId).Include(x => x.Customer).ThenInclude(x => x.AppUser).ToListAsync();
        }

        public Hotel GetHotelByIdForAddAmenity (string id)
        {
            return _context.Hotels.FirstOrDefault(x => x.Id == id);

        }

        public async Task<Hotel> GetHotelById (string hotelId)
        {
            var hotel = await _context.Hotels
                .Include(x => x.Galleries)
                .Where(x => x.Id == hotelId).FirstOrDefaultAsync();
            return hotel;
        }

        public IQueryable<Review> GetAllReviewsByHotelAsync (string hotelId)
        {
            var query = _context.Reviews.AsNoTracking().Where(h => h.HotelId == hotelId).Include(h => h.Hotel)
                .OrderBy(r => r.CreatedAt);
            return query;
        }
    }
}
