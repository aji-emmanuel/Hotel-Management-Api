using hotel_booking_models.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_api.Extensions
{
    public static class MailServiceExtension
    {
        public static void ConfigureMailService(this IServiceCollection services, IConfiguration Configuration)
        {
            var emailConfiguratioin = new MailSettings
            {
                Mail = Configuration["MailSettings:Mail"],
                DisplayName = Configuration["MailSettings:DisplayName"],
                Password = Configuration["MailSettings:Password"],
                Host = Configuration["MailSettings:Host"],
                Port = Convert.ToInt32(Configuration["MailSettings:Port"])
            };

            services.AddSingleton(emailConfiguratioin);
                
        }

    }
}
