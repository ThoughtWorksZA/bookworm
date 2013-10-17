using System.Net;
using System.Net.Mail;
using System.Text;
using BookWorm.Services.Email;
using FluentAssertions;
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

        [TestMethod]
        public void ShouldConfigureMailMessageCorrectly()
        {
            var smtpClientWrapper = new Mock<SmtpClientWrapper>();
            smtpClientWrapper.Setup(it => it.Send(It.IsAny<MailMessage>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(),
                        It.IsAny<NetworkCredential>()));
            var emailService = new EmailService(smtpClientWrapper.Object);

            emailService.SendConfirmation("from@thoughtworks.com", "to@thoughtworks.com", "security", 1);

            smtpClientWrapper.Verify(it => it.Send(It.Is<MailMessage>(mail => AssertMailIsCorrectlyConfigured(mail)), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(),
                        It.IsAny<NetworkCredential>()));
        }

        private bool AssertMailIsCorrectlyConfigured(MailMessage mail)
        {
            mail.From.Should().Be("from@thoughtworks.com");
            mail.To.Should().Contain("to@thoughtworks.com");
            mail.To.Count.Should().Be(1);
            mail.BodyEncoding.Should().Be(Encoding.UTF8);
            mail.DeliveryNotificationOptions.Should().Be(DeliveryNotificationOptions.OnFailure);
            return true;
        }

        [TestMethod]
        public void ShouldSetContentOfMailMessageCorrectly()
        {
            var smtpClientWrapper = new Mock<SmtpClientWrapper>();
            smtpClientWrapper.Setup(it => it.Send(It.IsAny<MailMessage>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(),
                        It.IsAny<NetworkCredential>()));
            var emailService = new EmailService(smtpClientWrapper.Object);

            emailService.SendConfirmation("from@thoughtworks.com", "to@thoughtworks.com", "security", 1);

            smtpClientWrapper.Verify(it => it.Send(It.Is<MailMessage>(mail => AssertMailContentIsCorrect(mail)), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(),
                        It.IsAny<NetworkCredential>()));
        }

        private bool AssertMailContentIsCorrect(MailMessage mail)
        {
            const string expectedBody = "Dear User,\r\nThe administrator of PUKU has created an account for you. To complete the registration process click on this link\r\nhttp://puku.co.za/Users/1/RegisterConfirmation/security";
            mail.Subject.Should().Be("The administror of PUKU created a user for you");
            mail.Body.Should().Be(expectedBody);
            return true;
        }
    }
}
