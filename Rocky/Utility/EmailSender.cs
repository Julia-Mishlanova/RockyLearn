using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Rocky_Models;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Rocky.Interfaces;

namespace Rocky.Utility
{
    public class EmailSender : IMailSender
    {
        public void SendEmail(string AdminEmail, MimeMessage message, string messageBody)
        {
            // Создаем HTML-тело сообщения
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = messageBody;

            // Прикрепляем HTML-тело к сообщению
            message.Body = bodyBuilder.ToMessageBody();
            //я тебя люблю!!!!!!
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var val = configuration["pw"];
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate(AdminEmail, val);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
