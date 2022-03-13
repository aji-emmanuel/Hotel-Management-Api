using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IHotelStatisticsService
    {
        Task<Response<AdminStatisticsDto>> GetAdminStatistics();

        Task<Response<HotelStatisticDto>> GetHotelStatistics(string hotelId);
        
        Task<Response<HotelManagerStatisticsDto>> GetHotelManagerStatistics(string managerId);
    }
}
