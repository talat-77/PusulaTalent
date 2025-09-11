using SchoolManangement.Business.Dto;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace SchoolManangement.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, null, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(tos, subject, body, null, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, List<EmailAttachment> attachments, bool isBodyHtml = true)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_configuration["Mail:FromMail"], _configuration["Mail:Username"]));

            foreach (var to in tos)
            {
                email.To.Add(MailboxAddress.Parse(to));
            }

            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = isBodyHtml ? body : null,
                TextBody = isBodyHtml ? null : body
            };
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    builder.Attachments.Add(attachment.FileName, attachment.FileContent);
                }
            }

            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _configuration["Mail:Host"],
                int.Parse(_configuration["Mail:Port"]),
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
               _configuration["Mail:Username"],
               _configuration["Mail:Password"]);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

        }

        public async Task SendMailAsync(string to, string subject, string body, List<EmailAttachment> attachments, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, attachments, isBodyHtml);
        }
    }
}
