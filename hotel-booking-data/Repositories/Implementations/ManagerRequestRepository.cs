using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class ManagerRequestRepository : GenericRepository<ManagerRequest>, IManagerRequestRepository
    {
        private readonly DbSet<ManagerRequest> _dbSet;
        private readonly HbaDbContext _context;
        public ManagerRequestRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<ManagerRequest>();
        }

        public async Task<ManagerRequest> GetHotelManagerRequestByEmail(string email)
        {
            var check = await _dbSet.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            return check;
        }

        public async Task<ManagerRequest> GetHotelManagerByEmailToken(string email, string token)
        {
            var check = await _dbSet.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            var checkToken = check != null && check.Token == token;
            return checkToken ? check : null;
        }

        public IQueryable<ManagerRequest> GetManagerRequest()
        {
            var check =  _dbSet.Select(x => x)
                .OrderByDescending(recent => recent.CreatedAt)
                .OrderBy(recent => recent.ConfirmationFlag);
            return check;
        }


    }
}
