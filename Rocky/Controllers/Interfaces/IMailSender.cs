using MimeKit;

namespace Rocky.Controllers.Interfaces
{
    public interface IMailSender
    {
        void SendEmail(string v, MimeMessage message, string messageBody);
    }
}
