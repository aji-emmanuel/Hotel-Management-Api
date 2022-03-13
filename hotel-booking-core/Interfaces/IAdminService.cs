using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace hotel_booking_core.Interfaces
{
    public interface IAdminService
    {
        Task<Response<PageResult<IEnumerable<TransactionResponseDto>>>> GetManagerTransactionsAsync(string managerId, TransactionFilter filter);
        Task<Response<PageResult<IEnumerable<TransactionResponseDto>>>> GetAllTransactions(TransactionFilter filter);
    }
}
