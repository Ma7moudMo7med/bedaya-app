using Infrastructure.Services.ForgotPassword;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Infrastructure.Services.ForgotPassword
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config) => _config = config;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var host = _config["SMTP:Host"];
            var port = int.Parse(_config["SMTP:Port"]);
            var user = _config["SMTP:User"];
            var pass = _config["SMTP:Pass"];
            var from = _config["SMTP:From"];

            using var smtp = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };

            var msg = new MailMessage(from, email, subject, htmlMessage) { IsBodyHtml = true };
            await smtp.SendMailAsync(msg);
        }
    }
}
