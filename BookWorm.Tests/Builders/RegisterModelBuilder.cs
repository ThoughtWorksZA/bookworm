using BookWorm.Models;

namespace BookWorm.Tests.Builders
{
    public class RegisterModelBuilder
    {
        private const string DefaultPassword = RegisterModel.DefaultPassword;
        private readonly RegisterModel _registerModel = new RegisterModel()
            {
                UserName = "Akani@thoughtworks.com",
                Password = DefaultPassword,
                ConfirmPassword = DefaultPassword,
                Role = Roles.Admin
            };

        public RegisterModel Build()
        {
            return _registerModel;
        }

        public RegisterModelBuilder WithUserName(string username)
        {
            _registerModel.UserName = username;
            return this;
        }

        public RegisterModelBuilder WithRole(string role)
        {
            _registerModel.Role = role;
            return this;
        }
    }
}