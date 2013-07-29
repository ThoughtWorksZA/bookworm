using BookWorm.Models;

namespace BookWorm.Tests.Builders
{
    public class RegisterModelBuilder
    {
        private readonly RegisterModel _registerModel = new RegisterModel()
            {
                UserName = "Akani@thoughtworks.com",
                Password = "111111",
                ConfirmPassword = "111111",
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