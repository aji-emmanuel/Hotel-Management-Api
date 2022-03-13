using hotel_booking_data.Contexts;
using hotel_booking_models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Seeder
{
    public class HbaSeeder
    {
        public static async Task SeedData(HbaDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var baseDir = Directory.GetCurrentDirectory();
           
            await dbContext.Database.EnsureCreatedAsync();
            
            if (!dbContext.Users.Any())
            {
                List<string> roles = new List<string> { "Admin", "Manager", "Customer" };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }

                var user = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Hotel",
                    LastName = "Admin",
                    UserName = "hbapp",
                    Email = "info@hba.com",
                    PhoneNumber = "09043546576",
                    Gender = "Male",
                    Age = 34,
                    IsActive = true,
                    PublicId = null,
                    Avatar = "http://placehold.it/32x32",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                user.EmailConfirmed = true;
                await userManager.CreateAsync(user, "Password@123");
                await userManager.AddToRoleAsync(user, "Admin");

                var path = File.ReadAllText(FilePath(baseDir, "Json/users.json"));

                var hbaUsers = JsonConvert.DeserializeObject<List<AppUser>>(path);
                for (int i = 0; i < hbaUsers.Count; i++)
                {
                    hbaUsers[i].EmailConfirmed = true;
                    await userManager.CreateAsync(hbaUsers[i], "Password@123");
                    if (i < 5)
                    {
                        await userManager.AddToRoleAsync(hbaUsers[i], "Manager");
                        continue;
                    }
                    await userManager.AddToRoleAsync(hbaUsers[i], "Customer");
                }
            }

            
            // Bookings and Payment
            if (!dbContext.Bookings.Any())
            {
                var path = File.ReadAllText(FilePath(baseDir, "Json/bookings.json"));

                var bookings = JsonConvert.DeserializeObject<List<Booking>>(path);
                await dbContext.Bookings.AddRangeAsync(bookings);
            }

            // Hotels, roomtypes n rooms
            if (!dbContext.Hotels.Any())
            {
                var path = File.ReadAllText(FilePath(baseDir, "Json/Hotel.json"));

                var hotels = JsonConvert.DeserializeObject<List<Hotel>>(path);
                await dbContext.Hotels.AddRangeAsync(hotels);
            }

            // Whishlist
            if (!dbContext.WishLists.Any())
            {
                var path = File.ReadAllText(FilePath(baseDir, "Json/wishlists.json"));

                var wishList = JsonConvert.DeserializeObject<List<WishList>>(path);
                await dbContext.WishLists.AddRangeAsync(wishList);
            }


            await dbContext.SaveChangesAsync();
        }

         static string FilePath(string folderName, string fileName)
            {
                return Path.Combine(folderName, fileName);
            }
    }
}
