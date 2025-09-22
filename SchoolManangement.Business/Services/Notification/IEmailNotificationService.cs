using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Business.Services.Notification
{
    public interface IEmailNotificationService
    {
        Task SendWelcomeEmailAsync(string email, string username, string password, string firstName, string lastName);
       

    }
}
