using Microsoft.AspNetCore.Http;
using SchoolManangement.Business.Dto;
using SchoolManangement.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Emails.Commands.SendEmailMultiples.Commands.SendEmailMultiple
{
    public class SendEmailToMultipleUsersCommandHandler : ICommandHandler<SendEmailToMultipleUsersCommand, bool>
    {
        private readonly IEmailService _emailService;

        public SendEmailToMultipleUsersCommandHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<bool> Handle(SendEmailToMultipleUsersCommand request, CancellationToken cancellationToken)
        {
            var attachments = new List<EmailAttachment>();

            if (request.Attachments != null)
            {
                foreach (var file in request.Attachments)
                {
                    var attachment = new EmailAttachment
                    {
                        FileName = file.FileName,
                        FileContent = await ConvertFileToByteArrayAsync(file),
                        ContentType = file.ContentType
                    };

                    attachments.Add(attachment);
                }
            }

            foreach (var recipient in request.Tos)
            {
                await _emailService.SendMailAsync(
                    recipient,
                    request.Subject,
                    request.Body,
                    attachments,
                    request.IsBodyHtml
                );
            }

            return true;
        }

        private async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
