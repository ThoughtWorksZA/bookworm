using System.Net;
using System.Net.Mail;
using System.Text;

namespace BookWorm.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClientWrapper _smtpClient;

        public const string ConfirmationEmailSubject = "The administror of PUKU created a user for you";
        private const string Template = @"Dear User,
The administrator of PUKU has created an account for you. To complete the registration process click on this link
http://puku.co.za/Users/{0}/RegisterConfirmation/{1}";

        public EmailService(SmtpClientWrapper smtpClient = null)
        {
            _smtpClient = smtpClient ?? new SmtpClientWrapper();
        }

        public void SendConfirmation(string from, string to, string securityToken, int userId)
        {
            var mailMessage = new MailMessage(from, to, ConfirmationEmailSubject, string.Format(Template, userId, securityToken))
                {
                    BodyEncoding = Encoding.UTF8,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
            _smtpClient.Send(mailMessage, "smtp.gmail.com", 587, true, new NetworkCredential("pukusite@gmail.com", "b00ksRc00l"));
        }
    }
}
