using System;
using System.Net;
using System.Net.Mail;
using ToDoApp.Core.Service;

namespace ToDoApp.Infrastructure.Service
{
    public class MailNotificationService : INotificationService
    {
        private readonly string _smtpHost;
        private readonly string _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public MailNotificationService(string smtpHost, string smtpPort, string smtpUsername, string smtpPassword)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
        }

        public bool SendNotification(string messageFrom, string messageTo, string messageTitle, string messageBody)
        {
            MailMessage mail = new MailMessage(messageFrom, messageTo);
            SmtpClient client = new SmtpClient
            {
                Host = _smtpHost,
                Port = Convert.ToInt32(_smtpPort),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials =
                    new NetworkCredential(
                        _smtpUsername,
                        _smtpPassword
                        )
            };
            mail.Subject = messageTitle;
            mail.IsBodyHtml = true;
            mail.Body = messageBody;
            client.Send(mail);
            return true;
        }
    }
}
