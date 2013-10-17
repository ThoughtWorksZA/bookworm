using System.Net;
using System.Net.Mail;
using System.Text;
using BookWorm.Services;
using BookWorm.Services.Email;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Services
{
    [TestClass]
    public class EmailServiceTests
    {
        private Mock<SmtpClientWrapper> _smtpClientWrapper;
        private EmailService _emailService;
        private Mock<ConfigurationService> _configService;

        [TestInitialize]
        public void Setup()
        {
            _smtpClientWrapper = new Mock<SmtpClientWrapper>();
            _smtpClientWrapper.Setup(it => it.Send(It.IsAny<MailMessage>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(),
                        It.IsAny<NetworkCredential>()));
            _configService = new Mock<ConfigurationService>();
            _configService.Setup(it => it.GetEmailSenderAddress()).Returns(string.Empty);
            _configService.Setup(it => it.GetEmailSenderPassword()).Returns(string.Empty);
            _emailService = new EmailService(_smtpClientWrapper.Object, _configService.Object);
        }

        [TestMethod]
        public void ShouldSendAnEmailUsingSmtpClientWrapper()
        {
            _emailService.SendConfirmation("from@thoughtworks.com", "to@thoughtworks.com", "security", 1);

            _smtpClientWrapper.Verify(it => it.Send(It.IsAny<MailMessage>(), "smtp.gmail.com", 587, true,
                It.IsAny<NetworkCredential>()));
        }

        [TestMethod]
        public void ShouldConfigureMailMessageCorrectly()
        {
            _emailService.SendConfirmation("from@thoughtworks.com", "to@thoughtworks.com", "security", 1);

            _smtpClientWrapper.Verify(it => it.Send(It.Is<MailMessage>(mail => AssertMailIsCorrectlyConfigured(mail)), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(),
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
            _emailService.SendConfirmation("from@thoughtworks.com", "to@thoughtworks.com", "security", 1);

            _smtpClientWrapper.Verify(it => it.Send(It.Is<MailMessage>(mail => AssertMailContentIsCorrect(mail)), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(),
                        It.IsAny<NetworkCredential>()));
        }

        private bool AssertMailContentIsCorrect(MailMessage mail)
        {
            const string expectedBody = @"Dear User,
The administrator of PUKU has created an account for you. To complete the registration process click on this link
http://puku.co.za/Users/1/RegisterConfirmation/security";
            mail.Subject.Should().Be("The administror of PUKU created a user for you");
            mail.Body.Should().Be(expectedBody);
            return true;
        }

        [TestMethod]
        public void ShouldGetNetworkCredentialsFromConfigurationService()
        {
            _configService.Setup(it => it.GetEmailSenderAddress()).Returns("email");
            _configService.Setup(it => it.GetEmailSenderPassword()).Returns("password");

            _emailService.SendConfirmation("from@thoughtworks.com", "to@thoughtworks.com", "security", 1);

            _smtpClientWrapper.Verify(it => it.Send(It.IsAny<MailMessage>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(),
                        It.Is<NetworkCredential>(credential => credential.UserName == "email" && credential.Password == "password")));
        }
    }
}
