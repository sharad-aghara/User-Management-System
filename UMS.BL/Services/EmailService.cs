using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.BL.Configuration;
using UMS.Core.Interface;

namespace UMS.BL.Services
{
    public class EmailService : IEmailService
    {
        private readonly SMTPConfig _smtpConfig;

        public EmailService(IOptions<SMTPConfig> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        public async Task NotifyUserProfileApproved(string userEmail, string password)
        {
            var subject = "Your Profile Has Been Approved!";
            var body = $@"
                <p>Congratulations! Your profile has been approved.</p>
                <p>Your OTP is: <b>{password}</b></p>";

            var result = await SendEmailAsync(userEmail, subject, body);

            if (result)
            {
                Console.WriteLine("Email sent successfully.");
            }
            else
            {
                Console.WriteLine("Failed to send email.");
            }
        }

        public async Task NotifyUserProfileNotApproved(string userEmail)
        {
            var subject = "Your Profile Has Been Rejected!";
            var body = "<p>Sorry, Your profile has been rejected.</p>";

            var result = await SendEmailAsync(userEmail, subject, body);

            if (result)
            {
                Console.WriteLine("Email sent successfully.");
            }
            else
            {
                Console.WriteLine("Failed to send email.");
            }
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("User Profile Approved", _smtpConfig.Username));
                email.To.Add(new MailboxAddress("", to));
                email.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };
                email.Body = bodyBuilder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    var options = _smtpConfig.EnableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None;

                    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, options);
                    await smtp.AuthenticateAsync(_smtpConfig.Username, _smtpConfig.Password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        //public async Task<bool> SendFullEmailAsync(string to, string subject, string body, string cc, string bcc)
        //{
        //    try
        //    {
        //        var email = new MimeMessage();
        //        email.From.Add(new MailboxAddress("User Profile Approved", _smtpConfig.Username));
        //        email.To.Add(new MailboxAddress("", to));
        //        email.Subject = subject;

        //        if (cc != null && cc.Any())
        //        {
        //            List<string> ccList = cc.Split(';').Select(cc => cc.Trim()).ToList();

        //            foreach (var ccEmail in ccList)
        //            {
        //                email.Cc.Add(new MailboxAddress("", ccEmail));
        //            }
        //        }


        //        if (bcc != null && bcc.Any())
        //        {
        //            List<string> bccList = bcc.Split(';').Select(bcc => bcc.Trim()).ToList();

        //            foreach (var bccEmail in bccList)
        //            {
        //                email.Bcc.Add(new MailboxAddress("", bccEmail));
        //            }
        //        }

        //        var bodyBuilder = new BodyBuilder
        //        {
        //            HtmlBody = body
        //        };
        //        email.Body = bodyBuilder.ToMessageBody();

        //        using (var smtp = new SmtpClient())
        //        {
        //            var options = _smtpConfig.EnableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None;

        //            await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, options);
        //            await smtp.AuthenticateAsync(_smtpConfig.Username, _smtpConfig.Password);
        //            await smtp.SendAsync(email);
        //            await smtp.DisconnectAsync(true);
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
    }
}
