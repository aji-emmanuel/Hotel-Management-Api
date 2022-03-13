using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_booking_api.Extensions
{
    public static class CloudinaryServiceExtension
    {
        public static IServiceCollection AddCloudinary(this IServiceCollection services,
            Account account, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            services.Add(new ServiceDescriptor(typeof(Cloudinary), c => new Cloudinary(account), lifetime));
            return services;
        }

        public static Account GetAccount(IConfiguration Configuration)
        {
            Account account = new(
                                Configuration["ImageUploadSettings:CloudName"],
                                Configuration["ImageUploadSettings:ApiKey"],
                                Configuration["ImageUploadSettings:ApiSecret"]);
            return account;
        }
    }
}
