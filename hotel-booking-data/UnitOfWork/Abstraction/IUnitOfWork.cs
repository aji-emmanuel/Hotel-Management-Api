using hotel_booking_data.Repositories.Abstractions;
using System;
using System.Threading.Tasks;

namespace hotel_booking_data.UnitOfWork.Abstraction
{
    public interface IUnitOfWork : IDisposable
    {
        IAmenityRepository Amenities { get; }
        ICustomerRepository Customers { get; }
        IHotelRepository Hotels { get; }
        IManagerRepository Managers { get; }
        IPaymentRepository Payments { get; }
        IRoomRepository Rooms { get; }
        IWishListRepository WishLists { get; }
        IRoomTypeRepository RoomType { get; }
        IBookingRepository Booking {  get; }
        IReviewRepository Reviews { get; }
        IManagerRequestRepository ManagerRequest { get; }
        ITransactionRepository Transactions { get; }
        IRatingRepository Rating { get; }
        Task Save();
    }
}
