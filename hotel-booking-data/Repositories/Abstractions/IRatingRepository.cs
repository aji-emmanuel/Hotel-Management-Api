using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        Task<Rating> GetRatingsByHotel(string hotelId, string customerId);
    }
}
