using System;
using System.Configuration;

namespace BookWorm.Services
{
    public class ConfigurationService
    {
        public string GetEmailSenderAddress()
        {
            return ConfigurationManager.AppSettings.Get("EmailSenderAddress");
        }

        public string GetEmailSenderPassword()
        {
            return ConfigurationManager.AppSettings.Get("EmailSenderPassword");
        }
    }
}
