using hotel_booking_core.Interface;
using hotel_booking_models.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;


namespace hotel_booking_core.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger _logger;
        public MailService(MailSettings mailSettings, ILogger logger)

        {
            _logger = logger;
            _mailSettings = mailSettings;
        }


        public async Task<bool> SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage { Sender = MailboxAddress.Parse(_mailSettings.Mail) };
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                foreach (var file in mailRequest.Attachments.Where(file => file.Length > 0))
                {
                    byte[] fileBytes;
                    await using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    builder.Attachments.Add((file.FileName + Guid.NewGuid().ToString()), fileBytes, ContentType.Parse(file.ContentType));
                }
            }

            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            try
            {
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Source, e.InnerException, e.Message, e.ToString());
                return false;
            }

        }
    }
}

