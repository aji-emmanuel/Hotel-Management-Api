using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using hotel_booking_utilities.Comparer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {

        private readonly HbaDbContext _context;
        private readonly DbSet<Customer> _customers;

        public CustomerRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _customers = _context.Set<Customer>();
        }

        public Customer GetCustomer(string id)
        {
            Customer customer = _customers.Find(id);
            return customer;
        }

        public async Task<Customer> GetCustomerAsync(string id)
        {
            return await _customers
                .Include(x => x.AppUser)
                .Include(x => x.Bookings)
                .FirstOrDefaultAsync(x => x.AppUserId == id);
        }

        public IQueryable<Customer> GetAllUsers()
        {
            return _customers.Include(x => x.AppUser);
        }

        public async Task<IEnumerable<Customer>> GetTopCustomerForManagerAsync(string managerId)
        {
            var query = _customers.AsNoTracking()
                        .Where(c => c.Bookings.Where(bk => bk.Hotel.ManagerId == managerId).Count() > 0)
                        .Include(c => c.AppUser)
                        .Include(c => c.Bookings.Where(x => x.Hotel.ManagerId == managerId && x.PaymentStatus == true)).ThenInclude(bk => bk.Hotel)
                        .Include(c => c.Bookings.Where(x => x.Hotel.ManagerId == managerId && x.PaymentStatus == true)).ThenInclude(bk => bk.Payment);

            if (query.Count() == 0) return null;

            var topMoneySpenders = await query.OrderByDescending(c => c.Bookings.Sum(bk => bk.Payment.Amount)).Take(3).ToListAsync();

            var topFrequentUsers = await query.OrderByDescending(c => c.Bookings.Count).Take(5).ToListAsync();

            topMoneySpenders.AddRange(topFrequentUsers);

            return topMoneySpenders.Distinct(new CustomerHotelIdComparer()).Take(5);
        }//end GetTopCustomerForManagerAsync

        public async Task<Customer> GetCustomerDetails(string id)
        {
            return await _customers.Where(c => c.AppUserId == id)
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync();
        }
    }
}

