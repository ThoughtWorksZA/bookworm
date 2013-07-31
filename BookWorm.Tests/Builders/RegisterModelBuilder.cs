using BookWorm.Models;

namespace BookWorm.Tests.Builders
{
    public class RegisterModelBuilder
    {
        private const string DefaultPassword = RegisterModel.DefaultPassword;
        private readonly RegisterModel _registerModel = new RegisterModel()
            {
                Email = "Akani@thoughtworks.com",
                Password = DefaultPassword,
                ConfirmPassword = DefaultPassword,
                Role = Roles.Admin
            };

        public RegisterModel Build()
        {
            return _registerModel;
        }

        public RegisterModelBuilder WithEmail(string email)
        {
            _registerModel.Email = email;
            return this;
        }

        public RegisterModelBuilder WithRole(string role)
        {
            _registerModel.Role = role;
            return this;
        }
    }
}