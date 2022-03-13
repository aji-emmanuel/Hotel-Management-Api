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
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {

        private readonly HbaDbContext _context;
        private readonly DbSet<Review> _reviews;

        public ReviewRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _reviews = _context.Set<Review>();
        }


        public async Task<Review> CheckHotelExistence(string hotelId)
        {

            return await _reviews.Where(x =>x.HotelId == hotelId).FirstOrDefaultAsync();
        }

       
        public Review GetUserReview(string reviewId)
        {
            var custonerReview = _context.Reviews.SingleOrDefault(x => x.Id == reviewId);

            return custonerReview;
        }

       
        public IQueryable<Review> GetAllReviewsByHotelAsync(string hotelId)
        {
            var query = _context.Reviews.AsNoTracking().Where(h => h.HotelId == hotelId)
                .Include(h => h.Hotel)
                .Include(h => h.Customer.AppUser)
                .OrderBy(r => r.CreatedAt);
            return query;
        }

        
    }
}
