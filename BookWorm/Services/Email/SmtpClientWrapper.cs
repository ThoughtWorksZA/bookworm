using System.Net;
using System.Net.Mail;

namespace BookWorm.Services.Email
{
    public class SmtpClientWrapper
    {
        public virtual void Send(MailMessage mailMessage, string host, int port, bool enableSsl,
            NetworkCredential networkCredential)
        {
            var smtpClient = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = enableSsl,
                Credentials = networkCredential
            };

            smtpClient.Send(mailMessage);
        }
    }
}