using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_models.Cloudinary;
using hotel_booking_utilities.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface ICustomerService
    {
        Task<Response<string>> UpdateCustomer(string CustomerId, UpdateCustomerDto updateCustomer);
        Task<Response<UpdateImageDto>> UpdatePhoto(AddImageDto imageDto, string userId);
        Task<Response<PageResult<IEnumerable<GetUsersResponseDto>>>> GetAllCustomersAsync(PagingDto pagenator);
        Task<Response<PageResult<IEnumerable<CustomerWishListDto>>>> GetCustomerWishList(string customerId, PagingDto paging);
        Task<Response<CustomerDetailsToReturnDto>> GetCustomerDetails(string userId);
    }
}