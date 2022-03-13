using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace hotel_booking_data.Repositories.Implementations
{
    public class TokenRepository : ITokenRepository
    {
        private readonly HbaDbContext _context;
        public TokenRepository(HbaDbContext context)
        {
            _context = context;
        }

        
        public async Task<AppUser> GetUserByRefreshToken(Guid token, string userId)
        {
            //Check for user Id

            var user =  await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == token && u.Id == userId);
            
            if (user == null)
            {
                throw new ArgumentException($"User with Id {userId} does not exist");
            }

            return user;
        }
    }
}
