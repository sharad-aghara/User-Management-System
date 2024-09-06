using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.Interface
{
    public interface IEmailService
    {
        Task NotifyUserProfileApproved(string userEmail, String password);
        Task NotifyUserProfileNotApproved(string userEmail);
        Task<bool> SendEmailAsync(string to, string subject, string body);

        //Task<bool> SendFullEmailAsync(string to, string subject, string body, string cc, string bcc);
    }
}
