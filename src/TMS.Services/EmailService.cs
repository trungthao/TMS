using MimeKit;
using Microsoft.Extensions.Options;
using TMS.Domain.Constants;
using TMS.Domain.Services;
using MailKit.Net.Smtp;

namespace TMS.Services
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public void Send(string to, string subject, string html, string from = null)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(from ?? _appSettings.EmailFrom));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = html };

            using (var client = new SmtpClient ()) 
            {
                client.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}