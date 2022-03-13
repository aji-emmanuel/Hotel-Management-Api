using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class WishListRepository : GenericRepository<WishList>, IWishListRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<WishList> _dbSet;
        public WishListRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<WishList>();
        }

        public IQueryable<WishList> GetCustomerWishList(string customerId)
        {
            var customerWishlist = _dbSet.Where(x => x.CustomerId == customerId)
                                             .Include(x => x.Hotel)
                                             .ThenInclude(y => y.Galleries);
            return customerWishlist;

        }

        public async Task<WishList> CheckWishListAsync(string customerId, string hotelId)
        {
            return await _dbSet.Where(w => w.CustomerId == customerId)
                .Where(w => w.HotelId == hotelId)
                .FirstOrDefaultAsync();
        }
    }
}
