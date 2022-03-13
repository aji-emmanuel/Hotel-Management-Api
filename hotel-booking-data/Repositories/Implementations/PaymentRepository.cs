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
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<Payment> _dbSet;

        public PaymentRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Payment>();
        }

        public async Task<Payment> GetPaymentByReference(string transactionReference)
        {
            return await _context.Payments
                .Where(p => p.TransactionReference == transactionReference)
                .Include(p => p.Booking)
                .ThenInclude(b => b.Room)
                .FirstOrDefaultAsync();
        }
        public IQueryable<Payment> GetHotelTransactions(string hotelId)
        {
            var query = _dbSet.AsNoTracking()
                .Include(payment => payment.Booking)
                .ThenInclude(x => x.Hotel)
                .Where(x => x.Booking.HotelId == hotelId);
            return query;
        }


    }
}
