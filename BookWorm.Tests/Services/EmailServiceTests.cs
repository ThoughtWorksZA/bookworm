using System.Net;
using System.Net.Mail;
using BookWorm.Services.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Services
{
    [TestClass]
    public class EmailServiceTests
    {
        [TestMethod]
        public void ShouldSendAnEmailUsingSmtpClientWrapper()
        {
            var smtpClientWrapper = new Mock<SmtpClientWrapper>();
            smtpClientWrapper.Setup(it => it.Send(It.IsAny<MailMessage>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(),
                        It.IsAny<NetworkCredential>()));
            var emailService = new EmailService(smtpClientWrapper.Object);

            emailService.SendConfirmation("from@thoughtworks.com", "to@thoughtworks.com", "security", 1);

            smtpClientWrapper.Verify(it => it.Send(It.IsAny<MailMessage>(), "smtp.gmail.com", 587, true,
                It.Is<NetworkCredential>(credential => credential.UserName == "pukusite@gmail.com" && credential.Password == "b00ksRc00l")));
        }
    }
}
