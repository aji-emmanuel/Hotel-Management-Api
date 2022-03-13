using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static hotel_booking_utilities.Pagination.Paginator;

namespace hotel_booking_core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public AdminService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<PageResult<IEnumerable<TransactionResponseDto>>>> GetManagerTransactionsAsync(string managerId, TransactionFilter filter)
        {
            var manager = await _unitOfWork.Managers.GetManagerAsync(managerId);
            var response = new Response<PageResult<IEnumerable<TransactionResponseDto>>>();
            IQueryable<Booking> managerBookings;

            if (manager != null)
            {

                managerBookings = _unitOfWork.Booking.GetManagerBookings(managerId, filter);
                var transactionList = await managerBookings.PaginationAsync<Booking, TransactionResponseDto>(filter.PageSize, filter.PageNumber, _mapper);

                response.Message = "Transactions Fetched";
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Succeeded = true;
                response.Data = transactionList;
                return response;

            }
            return Response<PageResult<IEnumerable<TransactionResponseDto>>>.Fail("Manager not found");
        }

        public async Task<Response<PageResult<IEnumerable<TransactionResponseDto>>>> GetAllTransactions(TransactionFilter filter)
        {
            var transactions = _unitOfWork.Transactions.GetAllTransactions(filter);
            var item = await transactions.PaginationAsync<Booking, TransactionResponseDto>(filter.PageSize, filter.PageNumber, _mapper);
            return new Response<PageResult<IEnumerable<TransactionResponseDto>>>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Succeeded = true,
                Data = item,
                Message = "All transactions retrieved successfully"
            };
        }
    }
}
