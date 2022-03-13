using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_models;
using Serilog;
using System.Net;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        

        public ReviewService(IUnitOfWork unitOfWork,IMapper mapper, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;

        }

        public Response<string> DeleteUserReview(string customerId, string reviewId)
        {
            _logger.Information("Getting review by id");
            var review = _unitOfWork.Reviews.GetUserReview(reviewId);
            var response = new Response<string>();

            if (review != null)
            {
                if (review.CustomerId == customerId)
                {
                    _unitOfWork.Reviews.Delete(review);
                    _unitOfWork.Save();
                    response.Succeeded = true;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Message = $"Review deleted successfully";
                    _logger.Information("Review deleted successfully");

                    return response;

                }
                _logger.Information("User is attempting to delete a review that does not belong to him/her");
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.Message = $"You are not authorized to delete this review";

                return response;
            }
            _logger.Information("Review does not exist");
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"Review does not exist";

            return response;
        }

        public Response<string> UpdateUserReview(string customerId, string reviewId, ReviewRequestDto model)
        {
            _logger.Information($"Attempt to update a review by {customerId}");
            var response = new Response<string>();
            var review = _unitOfWork.Reviews.GetUserReview(reviewId);
                
            if (review.CustomerId == customerId)
            {
                review.Comment = model.Comment;
                _unitOfWork.Reviews.Update(review);
                _unitOfWork.Save();
                _logger.Information("Updated review successfully");
                

                response.Succeeded = true;
                response.StatusCode = (int)HttpStatusCode.Created;
                response.Message = $"Comment updated successfully";

                return response;
            }

            _logger.Information("Review update failed");
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"Review with the Id {reviewId} does not exist";

            return response;
        }

        public async Task<Response<AddReviewToReturnDto>> AddReviewAsync(AddReviewDto model, string customerId)
        {
            _logger.Information($"An attempt to add a review by customer{customerId}");
            var response = new Response<AddReviewToReturnDto>();


            var checkHotel = await _unitOfWork.Reviews.CheckHotelExistence(model.HotelId);
            if (checkHotel == null)
            {
                _logger.Information("Add review operation not successful");
                response.Succeeded = false;
                response.Message = "Hotel does not exist";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            //var previousReview = await _unitOfWork.Reviews.g
            var review = _mapper.Map<Review>(model);

            review.CustomerId = customerId;

            await _unitOfWork.Reviews.InsertAsync(review);
            await _unitOfWork.Save();

            var reviewToReturn = _mapper.Map<AddReviewToReturnDto>(review);
            _logger.Information("Added review successfully");

            //response
            response.Succeeded = true;
            response.Data = reviewToReturn;
            response.Message = "Review added successfully";
            response.StatusCode = (int)HttpStatusCode.OK;

            return response;
        }     
    }
}
