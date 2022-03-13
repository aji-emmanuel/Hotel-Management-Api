using Microsoft.Extensions.Configuration;
using System.IO;

namespace hotel_booking_api.Extensions
{
    public static class ConfigurationSetupExtension
    {
        public static IConfiguration GetConfig(bool isDevelopment)
        {
            return isDevelopment ? new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build()
                :
                new ConfigurationBuilder()
                   .AddEnvironmentVariables()
                   .Build();
        }
    }
}
