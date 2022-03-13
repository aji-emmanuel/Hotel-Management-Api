using AutoMapper;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.Mapper
{
    public class ManagerMapper : Profile
    {
        public ManagerMapper()
        {
            CreateMap<Manager, AdminStatisticsDto>().ReverseMap();
        }
    }
}
