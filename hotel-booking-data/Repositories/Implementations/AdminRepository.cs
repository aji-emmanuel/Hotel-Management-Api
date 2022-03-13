using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
namespace hotel_booking_data.Repositories.Implementations
{
    public class AdminRepository : GenericRepository<AppUser>, IAdminRepository
    {
        private readonly HbaDbContext _context;
        public AdminRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }
    }
}