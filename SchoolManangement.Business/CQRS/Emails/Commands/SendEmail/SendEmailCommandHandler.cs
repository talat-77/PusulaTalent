using Microsoft.AspNetCore.Http;
using SchoolManangement.Business.Dto;
using SchoolManangement.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Emails.Commands.SendEmail
{
    public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand, bool>
    {
        private readonly IEmailService _emailService;

        public SendEmailCommandHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<bool> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {

            var attachments = new List<EmailAttachment>();

            if (request.Attachments != null)
            {

                foreach (var file in request.Attachments)
                {
                    var attachment = new EmailAttachment
                    {
                        FileName = file.FileName,
                        FileContent = await ConvertFileToByteArrayAsync(file), //metot aşağıda tanımlı
                        ContentType = file.ContentType
                    };

                    attachments.Add(attachment);
                }
            }


            await _emailService.SendMailAsync(
                request.To,
                request.Subject,
                request.Body,
                attachments,
                request.IsBodyHtml
            );

            return true;
        }

        // Dosya içeriğini byte dizisine çevirmek için asenkron metod
        private async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }




    }
}
