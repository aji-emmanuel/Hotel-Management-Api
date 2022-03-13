using hotel_booking_dto;
using hotel_booking_dto.ReviewDtos;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IReviewService
    {

        Task<Response<AddReviewToReturnDto>> AddReviewAsync(AddReviewDto model, string customerId);
        Response<string> DeleteUserReview(string customerId, string reviewId);
        Response<string> UpdateUserReview(string customerId, string reviewId, ReviewRequestDto model);
        
    }
}