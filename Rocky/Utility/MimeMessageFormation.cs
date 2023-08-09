using MimeKit;
using Rocky.Controllers.Interfaces;

namespace Rocky.Utility
{
    public class MimeMessageFormation : IMessageFormater
    {
        public MimeMessage GetMimeMessage(string messageBody, MailboxAddress sender, MailboxAddress receiver)
        {
            var message = new MimeMessage();
            message.From.Add(sender);
            message.To.Add(receiver);
            message.Subject = "Subject";
            message.Body = new TextPart("plain")
            {
                Text = messageBody
            };

            return message;
        }
    }
}
