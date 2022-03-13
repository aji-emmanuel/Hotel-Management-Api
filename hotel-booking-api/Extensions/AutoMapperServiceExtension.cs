using hotel_booking_utilities.AutoMapSetup;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_booking_api.Extensions
{
    public static class AutoMapperServiceExtension
    {
        public static void ConfigureAutoMappers(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapInitializer));
        }
    }
}
