using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.Contexts;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class HotelStatisticsService : IHotelStatisticsService
    {
        private readonly HbaDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IMapper _mapper;

        public HotelStatisticsService(HbaDbContext db, IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

       private async Task<List<string>> GetAllManagers() 
        {
            var hotelObj = await _db.Hotels.ToListAsync();
            var managersIdList = from manager in hotelObj select manager.ManagerId;
            var managersList = new List<string>();
            var userObj = await _db.Users.ToListAsync();
            var usersIdList = from user in userObj select user.Id;

            foreach(var item in usersIdList) 
            {
                if (managersIdList.Contains(item)) 
                {
                    var manager = userObj.Where(x => x.Id == item).FirstOrDefault();
                    var managersName = $"{ manager.FirstName} {manager.LastName}";
                    managersList.Add(managersName);
                }
            }

            
            return managersList;
        }

       private async Task<decimal> GetAllTotalMonthlyTransactions() 
       {
            string transactionMonth = DateTime.Now.ToString("MM");
            decimal totalMonthlyPayments = 0;
            var payments = await _db.Payments.ToListAsync();
            foreach (var item in payments)
            {
                var paymentDate = item.CreatedAt.ToString();
                var paymentMonth = paymentDate.Substring(3, 2);
                if (transactionMonth == paymentMonth)
                {
                    totalMonthlyPayments += item.Amount;
                }
            }
            return totalMonthlyPayments;
        }

        private async Task<int> GetTotalHotels() 
        {
            var hotelsList = await _db.Hotels.ToListAsync();
            return hotelsList.Count();
        }


        private async Task<int> GetTotalRoomsInEachHotel(string hotelId)
        {
            var roomType = await _unitOfWork.RoomType.GetRoomTypesInEachHotel(hotelId);
            

            var totalRooms = 0;


            foreach (var item in roomType)
            {
                var roomTypeId = item.Id;
                var roomList = await _db.Rooms.Where(x => x.RoomTypeId == roomTypeId).ToListAsync();
                var rooms = roomList.Count;

                totalRooms += rooms;

            }

            return totalRooms;

        }

        private async Task<int> GetTotalNoOfOccupiedRooms(string hotelId)
        {
            var roomType = await _unitOfWork.RoomType.GetRoomTypesInEachHotel(hotelId);
            
            var totalOccupiedRooms = 0;
            foreach (var item in roomType)
            {
                var roomTypeId = item.Id;
                var occupiedRoomsList = await _db.Rooms.Where(x => x.RoomTypeId == roomTypeId).
                                 Where(z => z.IsBooked == true).ToListAsync();
                var occupiedRooms = occupiedRoomsList.Count;

                totalOccupiedRooms += occupiedRooms;
            }
            return totalOccupiedRooms;
        }

        private async Task<decimal> GetTotalEarnings(string hotelId)
        {
            var hotelBookings = await _db.Bookings.Where(x => x.HotelId == hotelId).ToListAsync();

            decimal totalPayments = 0;

            foreach (var item in hotelBookings)
            {
                var bookingId = item.Id;
                var payment = _db.Payments.Where(x => x.BookingId == bookingId).ToList();
                foreach (var x in payment)
                {
                    totalPayments += x.Amount;
                }
            }

            return totalPayments;
        }

        private async Task<decimal> GetMonthlyEarnings(string hotelId) 
        {
            string sMonth = DateTime.Now.ToString("MM");
            var hotelBookings = await _db.Bookings.Where(x => x.HotelId == hotelId).ToListAsync();

            decimal totalMonthlyPayments = 0;

            foreach (var item in hotelBookings)
            {
                var bookingId = item.Id;
                var payment = await _db.Payments.Where(x => x.BookingId == bookingId).ToListAsync();

                foreach(var x in payment) 
                {
                    var paymentTime = x.CreatedAt.ToString();
                    var paymentMonth = paymentTime.Substring(3, 2);
                    if(sMonth == paymentMonth) 
                    {
                        totalMonthlyPayments += x.Amount;
                    }
                }
            }

            return totalMonthlyPayments;
        }

        private async Task<int> GetTotalNoOfVacantRooms(string hotelId)
        {
            var roomType = await _unitOfWork.RoomType.GetRoomTypesInEachHotel(hotelId);
            var totalOccupiedRooms = 0;
            foreach (var item in roomType)
            {
                var roomTypeId = item.Id;
                var unoccupiedRoomsList = await _db.Rooms.Where(x => x.RoomTypeId == roomTypeId).
                                 Where(z => z.IsBooked != true).ToListAsync();

                var unoccupiedRooms = unoccupiedRoomsList.Count;
                totalOccupiedRooms += unoccupiedRooms;
            }

            return totalOccupiedRooms;

        }

        private async Task<int> GetNoOfRoomTypes(string hotelId)
        {
            var roomType = await _unitOfWork.RoomType.GetRoomTypesInEachHotel(hotelId);
            return roomType.Count;
        }

        private async Task<int> GetTotalReviews(string hotelId)
        {
            var reviews = await _db.Reviews.Where(x => x.HotelId == hotelId).ToListAsync();
            return reviews.Count;
        }

        private async Task<int> GetTotalAmenities(string hotelId)
        {
            var amenities =  await _unitOfWork.Amenities.GetAmenityByHotelIdAsync(hotelId);
            return amenities.Count;
        }

        private async Task<int> GetTotalBookings(string hotelId)
        {
            var totalBookings = await _db.Bookings.Where(x => x.HotelId == hotelId).ToListAsync();
            return totalBookings.Count;
        }

        private async Task<double> GetAverageRatings(string hotelId)
        {
            var ratings = await _db.Ratings.Where(x => x.HotelId == hotelId).ToListAsync();
            double totalRatings = 0;
            foreach (var item in ratings)
            {
                totalRatings += Convert.ToDouble(item.Ratings);
            }

            var averageRatings = totalRatings / ratings.Count;
            return averageRatings;
        }

        private async Task<int> GetNoOfCustomers(string hotelId)
        {
            var noOfCustomers = await _db.Bookings.Where(x => x.HotelId == hotelId).ToListAsync();
            return noOfCustomers.Count;
        }
        private async Task<List<string>> GetAManagerHotels(string managerId)
        {
            var requests = await _db.Managers.Where(s => s.AppUserId == managerId)
            .Include(s => s.Hotels)
            .FirstOrDefaultAsync();
            List<string> result = new List<string>();
            foreach (var item in requests.Hotels)
            {
                result.Add(item.Id);
            }
            return result;
        }
        private async Task<Dictionary<string, decimal>> GetManagerTotalRevenueInAYear(string managerId)
        {
            //Get managers transactions
            var hotels = await GetAManagerHotels(managerId);
            List<List<Booking>> bookings = new List<List<Booking>>();



            foreach (var item in hotels)
            {
                var req = _db.Bookings.Where(e => DateTime.Now.AddMonths(-12) < e.CreatedAt)
                .Where(s => s.HotelId == item)
                .Include(s => s.Payment)
                .OrderByDescending(e => e.CreatedAt).ToList();
                bookings.Add(req);
            }


            //Now calculate the revenue

            Dictionary<string, decimal> result = new Dictionary<string, decimal>()
            {
                {"January", 0.0m},
                {"February", 0.0m},
                {"March", 0.0m},
                {"April", 0.0m},
                {"May", 0.0m},
                {"June", 0.0m},
                {"July", 0.0m},
                {"August", 0.0m},
                {"September", 0.0m},
                {"October", 0.0m},
                {"November", 0.0m},
                {"December", 0.0m}
                };



            foreach (var item in bookings)
            {

                foreach (var modl in item)
                {
                    string mm = modl.Payment.CreatedAt.ToString("MMMM");
                    decimal actual = modl.Payment.Amount * 0.90m;
                    result[mm] += actual;
                }
            }


            return result;
        }

        public async Task<Response<AdminStatisticsDto>> GetAdminStatistics()
        {

            var response = new Response<AdminStatisticsDto>();
            var totalHotels = await GetTotalHotels();
            var totalMonthlyTransactions = await GetAllTotalMonthlyTransactions();
            decimal commission = 0.1M;
            var totalMonthlyCommission = commission * totalMonthlyTransactions;
            var allManagers = await GetAllManagers();
            var annualRevenue = await GetAdminAnnualRevenue();


            var adminStatistics = new AdminStatisticsDto
            {
                TotalNumberOfHotels = totalHotels,
                TotalMonthlyTransactions = totalMonthlyTransactions,
                Commission = commission,
                TotalMonthlyCommission = totalMonthlyCommission,
                Managers = allManagers,
                AnnualRevenue = annualRevenue
            };

            if (adminStatistics != null)
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Succeeded = true;
                response.Data = adminStatistics;
                response.Message = $"are the statistics for the Admin Manager";
                return response;
            }

            response.Data = default;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = false;
            response.Message = $"No record exists in the database";
            return response;

        }
        public async Task<Response<HotelStatisticDto>> GetHotelStatistics(string hotelId)
        {
            var hotel = await _unitOfWork.Hotels.GetHotelById(hotelId);
            var response = new Response<HotelStatisticDto>();
            if (hotel != null)
            {
                var totalRooms = await GetTotalRoomsInEachHotel(hotelId);
                var occupiedRooms = await GetTotalNoOfOccupiedRooms(hotelId);
                var vacantRooms = await GetTotalNoOfVacantRooms(hotelId);
                var roomTypeCount = await GetNoOfRoomTypes(hotelId);
                var reviews = GetTotalReviews(hotelId);
                var noOfAmenities = await GetTotalAmenities(hotelId);
                var totalBookings = GetTotalBookings(hotelId);
                var averageRatings = GetAverageRatings(hotelId);
                var totalEarnings = GetTotalEarnings(hotelId);

                var hotelstatistics = new HotelStatisticDto
                {
                    Name = hotel.Name,
                    NumberOfRooms = totalRooms,
                    AverageRatings = await averageRatings,
                    RoomsOccupied = occupiedRooms,
                    RoomsUnOccupied = vacantRooms,
                    NumberOfReviews = await reviews,
                    TotalNumberOfBookings = await totalBookings,
                    TotalEarnings = await totalEarnings,
                    RoomTypes = roomTypeCount,
                    NumberOfAmenities = noOfAmenities
                };
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Succeeded = true;
                response.Data = hotelstatistics;
                response.Message = $"are the statistics for hotel with {hotel.Id}";
                return response;
            }

            response.Data = default;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = false;
            response.Message = $"Hotel with Id = { hotel.Id} doesn't exist";
            return response;
        }

        public async Task<Response<HotelManagerStatisticsDto>> GetHotelManagerStatistics(string managerId)
        {
            var managerStats = await _unitOfWork.Managers.GetManagerStatistics(managerId);
            var response = new Response<HotelManagerStatisticsDto>();

            var averageRating = 0.0;
            var numberOfHotels = 0;
            var totalNoOfCustomers = 0;
            var availableRooms = 0;
            var bookedRooms = 0;
            decimal monthlyTransactions = 0;
            var totalRooms = 0;
            var totalAnnualTransactions = new Dictionary<string, decimal>();

            if (managerStats != null)
            {
                var hotels = _db.Hotels.Where(x => x.ManagerId == managerId).ToList();
                numberOfHotels = hotels.Count;

                foreach (var hotel in hotels)
                {
                    var hotelId = hotel.Id;
                    averageRating += await GetAverageRatings(hotelId);
                    totalNoOfCustomers += await GetNoOfCustomers(hotelId);
                    totalRooms += await GetTotalRoomsInEachHotel(hotelId);
                    availableRooms += await GetTotalNoOfVacantRooms(hotelId);
                    bookedRooms += await GetTotalNoOfOccupiedRooms(hotelId);
                    monthlyTransactions += await GetMonthlyEarnings(hotelId);
                      
                }

                totalAnnualTransactions = await GetManagerTotalRevenueInAYear(managerId);

                var hotelManagerStats = new HotelManagerStatisticsDto
                {
                    TotalHotels = numberOfHotels,
                    AverageHotelRatings = averageRating,
                    TotalNumberOfCustomers = totalNoOfCustomers,
                    TotalRooms = totalRooms,
                    BookedRooms = bookedRooms,
                    AvailableRooms = availableRooms,
                    TotalMonthlyTransactions = monthlyTransactions,
                    TotalAnnualRevenue = totalAnnualTransactions

                };
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Succeeded = true;
                response.Data = hotelManagerStats;
                response.Message = $"are the statistics for the hotel manager with Id: {managerId}";
                return response;
            }

            response.Data = default;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = false;
            response.Message = $"No record exists for this manager";
            return response;

        }



        private async Task<Dictionary<string, decimal>> GetAdminAnnualRevenue() 
        {
            var hotels = await _db.Hotels.ToListAsync();
            List<string> listOfhotelsId = new List<string>();

            foreach(var item in hotels) 
            {
                listOfhotelsId.Add(item.Id);
            }

            List<List<Booking>> bookings = new List<List<Booking>>();
            foreach (var item in listOfhotelsId)
            {
                var req = _db.Bookings.Where(e => DateTime.Now.AddMonths(-12) < e.CreatedAt)
                .Where(s => s.HotelId == item)
                .Include(s => s.Payment)
                .OrderByDescending(e => e.CreatedAt).ToList();
                bookings.Add(req);
            }

            Dictionary<string, decimal> result = new Dictionary<string, decimal>()
            {
                {"January", 0.0m},
                {"February", 0.0m},
                {"March", 0.0m},
                {"April", 0.0m},
                {"May", 0.0m},
                {"June", 0.0m},
                {"July", 0.0m},
                {"August", 0.0m},
                {"September", 0.0m},
                {"October", 0.0m},
                {"November", 0.0m},
                {"December", 0.0m}
                };



            foreach (var item in bookings)
            {

                foreach (var modl in item)
                {
                    string mm = modl.Payment.CreatedAt.ToString("MMMM");
                    decimal actual = modl.Payment.Amount * 0.1m;
                    result[mm] += actual;
                }
            }

            return result;
        }













    }
}
