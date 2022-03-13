using hotel_booking_models;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> InitializePayment(decimal amount, Customer customer, string paymentService, string bookingId, string transactionRef);
        Task<bool> VerifyTransaction(string transactionRef, string paymentMethod, string transactionId = null);
    }
}
