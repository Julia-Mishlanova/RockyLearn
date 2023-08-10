using MimeKit;

namespace Rocky.Interfaces
{
    public interface IMessageFormater
    {
        public MimeMessage GetMimeMessage(string messageBody, MailboxAddress sender, MailboxAddress receiver)
        {
            return new MimeMessage();
        }
    }
}
