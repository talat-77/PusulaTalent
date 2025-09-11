using SchoolManangement.Business.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Business.Services
{
    public interface IEmailService
    {
        Task SendMailAsync(string to, string subject, string body, bool isbodyHtml = true);
        Task SendMailAsync(string[] tos, string subject, string body, bool isbodyHtml = true);
        Task SendMailAsync(string[] tos, string subject, string body, List<EmailAttachment> attachments, bool isBodyHtml = true);
        Task SendMailAsync(string to, string subject, string body, List<EmailAttachment> attachments, bool isBodyHtml = true);
    }
}
