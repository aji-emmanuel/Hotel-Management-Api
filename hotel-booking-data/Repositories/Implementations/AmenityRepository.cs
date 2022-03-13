using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
    {
        private readonly HbaDbContext _context;

        public AmenityRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            
        }

        public Amenity GetAmenityById(string id)
        {
            var amenity = _context.Amenities.FirstOrDefault(x => x.Id == id);
            return amenity;
        }



        public async Task<List<Amenity>> GetAmenityByHotelIdAsync(string hotelId)
        {
            var amenities = await _context.Amenities.Where(x => x.HotelId == hotelId).ToListAsync();
            return amenities;
        }
    }
}
