using hotel_booking_dto.ManagerDtos;
using hotel_booking_models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IManagerRepository : IGenericRepository<Manager>
    {
        Task<Manager> GetManagerStatistics(string managerId);
        Task<Manager> GetManagerAsync(string managerId);
        Task<IEnumerable<Hotel>> GetAllHotelsForManagerAsync(string managerId);
        Task<bool> AddManagerAsync(Manager manager);
        Task<Manager> GetAppUserByEmail(string email);
        IQueryable<Manager> GetHotelManagersAsync();
    }
}
