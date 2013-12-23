using System.Net;
using System.Net.Mail;
using System.Text;

namespace BookWorm.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClientWrapper _smtpClient;
        private ConfigurationService _configurationService;

        private const int Port = 587;
        public const string ConfirmationEmailSubject = "The administror of PUKU created a user for you";
        private const string Template = @"Dear User,
The administrator of PUKU has created an account for you. To complete the registration process click on this link
http://puku.co.za/Users/{0}/RegisterConfirmation/{1}";

        public EmailService(SmtpClientWrapper smtpClient = null, ConfigurationService configurationService = null)
        {
            _smtpClient = smtpClient ?? new SmtpClientWrapper();
            _configurationService = configurationService ?? new ConfigurationService();
        }

        public void SendConfirmation(string from, string to, string securityToken, int userId)
        {
            var mailMessage = new MailMessage(from, to, ConfirmationEmailSubject, string.Format(Template, userId, securityToken))
                {
                    BodyEncoding = Encoding.UTF8,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
            var networkCredential = new NetworkCredential(_configurationService.GetEmailSenderAddress(),
                _configurationService.GetEmailSenderPassword());

            _smtpClient.Send(mailMessage,
                _configurationService.GetEmailServerAddress(),
                Port,
                _configurationService.IsSslEnabledForEmail(),
                networkCredential);
        }
    }
}
