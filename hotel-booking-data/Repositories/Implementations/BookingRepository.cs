using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace hotel_booking_data.Repositories.Implementations
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly HbaDbContext _context;
        public BookingRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Booking> GetManagerBookings(string managerId, TransactionFilter filter)
        {
            var bookings = _context.Bookings
                .Where(x => x.Hotel.ManagerId == managerId)
                .Where(x => filter.Month == 0 || x.CreatedAt.Month == (filter.Month))
                .Where(x => x.CreatedAt.Year == (filter.Year))
                .Where(x => string.IsNullOrEmpty(filter.SearchQuery) || x.Hotel.Name.ToLower().Contains(filter.SearchQuery.ToLower())
            || x.ServiceName.ToLower().Contains(filter.SearchQuery.ToLower())
            || x.Payment.Status.ToLower().Contains(filter.SearchQuery.ToLower())
            || x.Payment.TransactionReference.ToLower().Contains(filter.SearchQuery.ToLower())
            || x.Payment.MethodOfPayment.ToLower().Contains(filter.SearchQuery.ToLower())
            || x.Payment.Amount.ToString().Contains(filter.SearchQuery)
            || x.Hotel.State.ToLower().Contains(filter.SearchQuery.ToLower()))
                .Include(b => b.Payment)
                .Include(b => b.Customer)
                .Include(b => b.Hotel)
                .OrderByDescending(booking => booking.CreatedAt);
            return bookings;
        }
        public IQueryable<Booking> GetBookingsByCustomerId(string customerId)
        {
            var query = _context.Bookings.AsNoTracking()
                .Where(b => b.CustomerId == customerId)
                .Include(b => b.Hotel)
                .Include(b => b.Payment)
                .Include(b => b.Room)
                .ThenInclude(r => r.Roomtype)
                .OrderBy(b => b.CreatedAt);
            return query;
        }
        public IQueryable<Booking> GetBookingsByHotelId(string hotelId)
        {
            var query = _context.Bookings.AsNoTracking()
                .Where(b => b.HotelId == hotelId)
                .Where(b => b.PaymentStatus == true)
                .Include(b => b.Customer)
                .ThenInclude(c => c.AppUser)
                .Include(b => b.Payment)
                .OrderByDescending(b => b.Payment.Amount);
            return query;
        }

        public IQueryable<Booking> GetBookingsByManagerId(string managerId)
        {
            var query = _context.Bookings.AsNoTracking()
                .Where(b => b.Hotel.ManagerId == managerId)
                .Include(b => b.Customer)
                .ThenInclude(c => c.AppUser)
                .Include(b => b.Hotel)
                .OrderByDescending(b => b.CreatedAt);
            return query;
        }
    }
}
