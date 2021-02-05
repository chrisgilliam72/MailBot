using MailScan.SQLiteModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MailScan
{
    public class MailHelper
    {
        private SMTPSettings _configuration;

        public MailHelper(SMTPSettings configuration)
        {
            _configuration = configuration;
        }

        public void SendAsync(string from, List<String> toAddresses, string subject, string content)
        {
            Task.Run(() =>
            {
                Send(from, toAddresses, subject, content);
            });
        }


        public bool Send(string from, List<String> toAddresses, string subject, string content)
        {
            try
            {
                var host = _configuration.Host;
                var port = _configuration.Port;
                var username = _configuration.Username;
                var password = _configuration.Password;

                var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(username, password)
                };

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(from);
                foreach (var address in toAddresses)
                    mailMessage.To.Add(address);

                mailMessage.Subject = subject;
                mailMessage.Body = content;
                //mailMessage.IsBodyHtml = true;

                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
