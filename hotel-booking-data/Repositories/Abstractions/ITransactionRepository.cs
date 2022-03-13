using hotel_booking_dto;
using hotel_booking_models;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface ITransactionRepository : IGenericRepository<Booking>
    {
        IQueryable<Booking> GetAllTransactions(TransactionFilter filter);
        Task<Manager> GetManagerById(string id);
    }
}
