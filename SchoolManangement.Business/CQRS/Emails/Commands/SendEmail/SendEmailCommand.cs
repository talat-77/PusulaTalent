using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Emails.Commands.SendEmail
{
    public class SendEmailCommand : ICommand<bool>
    {
        public required string To { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }

        public bool IsBodyHtml { get; set; } = true;

        public List<IFormFile>? Attachments { get; set; }
    }
}
