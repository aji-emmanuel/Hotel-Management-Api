using hotel_booking_models.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interface
{
    public interface IMailService
    {
        Task<bool> SendEmailAsync(MailRequest mailRequest);
    }
}
