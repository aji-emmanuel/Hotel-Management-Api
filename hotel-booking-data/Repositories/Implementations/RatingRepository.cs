using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<Rating> _dbSet;
        public RatingRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Rating>();
        }

        public async Task<Rating> GetRatingsByHotel(string hotelId, string customerId)
        {
            return await _context.Ratings.FirstOrDefaultAsync(x => x.HotelId == hotelId && x.CustomerId == customerId);
        }
    }
}
