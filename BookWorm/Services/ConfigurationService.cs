using System;
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

        public virtual string GetEmailServerAddress()
        {
            return ConfigurationManager.AppSettings.Get("EmailServerAddress");
        }

        public virtual bool IsSslEnabledForEmail()
        {
            return Boolean.Parse(ConfigurationManager.AppSettings.Get("EmailEnableSsl"));
        }
    }
}
