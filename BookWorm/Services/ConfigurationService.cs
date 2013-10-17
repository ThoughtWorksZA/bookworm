using System.Configuration;

namespace BookWorm.Services
{
    public class ConfigurationService
    {
        public virtual string GetEmailSenderAddress()
        {
            return ConfigurationManager.AppSettings.Get("EmailSenderAddress");
        }

        public virtual string GetEmailSenderPassword()
        {
            return ConfigurationManager.AppSettings.Get("EmailSenderPassword");
        }
    }
}
