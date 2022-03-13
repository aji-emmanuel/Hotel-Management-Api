using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hotel_booking_data.Repositories.Implementations;
using hotel_booking_models;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface ITokenRepository
    {
        Task<AppUser> GetUserByRefreshToken(Guid token, string userId);
    }
}
