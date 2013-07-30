using System;
using BookWorm.Models;

namespace BookWorm.Tests.Builders
{
    public class LocalPasswordModelBuilder
    {
        private readonly LocalPasswordModel _localPasswordModel = new LocalPasswordModel()
        {
            SecurityToken = Guid.NewGuid().ToString(),
            ConfirmPassword = "222222",
            NewPassword = "222222",
            UserId = 1
        };

        public LocalPasswordModel Build()
        {
            return _localPasswordModel;
        }

        public LocalPasswordModelBuilder WithSecurityToken(string securityToken)
        {
            _localPasswordModel.SecurityToken = securityToken;
            return this;
        }

        public LocalPasswordModelBuilder WithUserId(int userId)
        {
            _localPasswordModel.UserId = userId;
            return this;
        }

        public LocalPasswordModelBuilder WithPassword(string password)
        {
            _localPasswordModel.NewPassword = password;
            return this;
        }
    }
}