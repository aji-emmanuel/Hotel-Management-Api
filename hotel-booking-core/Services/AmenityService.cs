using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_models;
using hotel_booking_utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AmenityService( IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public Response<UpdateAmenityDto> UpdateAmenity(string id, UpdateAmenityDto model)
        {
            var amenity = _unitOfWork.Amenities.GetAmenityById(id);
            var response = new Response<UpdateAmenityDto>();
            if (amenity != null)
            {
                var updatedAmenity = _mapper.Map(model, amenity);
                _unitOfWork.Amenities.Update(updatedAmenity);
                _unitOfWork.Save();
                var result = _mapper.Map<UpdateAmenityDto>(updatedAmenity);

                response.Data = result;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Update successful";
                response.Succeeded = true;
                return response;
            }

            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = "Not found";
            response.Succeeded = false;
            return response;
        }

        public async Task<Response<AddAmenityResponseDto>> AddAmenity(string id, AddAmenityRequestDto model)
        {

            var hotel = _unitOfWork.Hotels.GetHotelByIdForAddAmenity(id);
            var response = new Response<AddAmenityResponseDto>();

            if (hotel != null)
            {
                var amenityToAdd = _mapper.Map<Amenity>(model);
                await _unitOfWork.Amenities.InsertAsync(amenityToAdd);
                await _unitOfWork.Save();
                var result = _mapper.Map<AddAmenityResponseDto>(amenityToAdd);
                response.Data = result;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Amenity added successfully";
                response.Succeeded = true;
                return response;

            }

            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = "No such hotel";
            response.Succeeded = false;
            return response;
        }

        public async Task<Response<IEnumerable<AmenityDto>>> GetAmenityByHotelIdAsync(string hotelId)
        {
            var response = new Response<IEnumerable<AmenityDto>>();
            var hotel = _unitOfWork.Hotels.GetHotelById(hotelId);
            if (hotel is null)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "Hotel does not exist";
                response.Succeeded = false;
                return response;
            }

            var amenities = await _unitOfWork.Amenities.GetAmenityByHotelIdAsync(hotelId);

            var amenitiesOfHotel = amenities;
            var amenityList = new List<AmenityDto>();
            foreach (var amenity in amenitiesOfHotel)
            {
                amenityList.Add(_mapper.Map<AmenityDto>(amenity));
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.Succeeded = true;
            response.Data = amenityList;
            response.Message = $"Rooms for {hotelId}";
            return response;

        }

        public async Task<Response<string>> DeleteAmenityAsync(string amenityId)
        {
            var amenity = _unitOfWork.Amenities.GetAmenityById(amenityId);

            var response = new Response<string>();

            if (amenity != null)
            {
                _unitOfWork.Amenities.Delete(amenity);
                await _unitOfWork.Save();

                response.Message = $"Amenity with Id = {amenity.Id} deleted successfully";
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Succeeded = true;
                return response;
            }
            response.Message = $"Amenity with Id = {amenityId} does not exist";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = false;
            return response;
        }
    }
}