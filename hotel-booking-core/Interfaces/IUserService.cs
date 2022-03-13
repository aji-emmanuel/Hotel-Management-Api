
﻿using hotel_booking_dto.AppUserDto;
using hotel_booking_dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hotel_booking_dto.CustomerDtos;
using Microsoft.AspNetCore.Mvc;
using hotel_booking_models.Cloudinary;

namespace hotel_booking_core.Interfaces

{
    public interface IUserService
    {

        Task<Response<string>> UpdateAppUser(string appUserId, UpdateAppUserDto updateAppUser);
        Task<Response<UpdateImageDto>> UpdateUserPhoto(AddImageDto imageDto, string userId);
    }
}

