using MimeKit;

namespace Rocky.Controllers.Interfaces
{
    public interface IMessageFormater
    {
        public MimeMessage GetMimeMessage(string messageBody, object sender, object receiver)
        {
            return new MimeMessage();
        }
    }
}
