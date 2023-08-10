using MimeKit;

namespace Rocky.Interfaces
{
    public interface IMailSender
    {
        void SendEmail(string v, MimeMessage message, string messageBody);
    }
}
