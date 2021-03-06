﻿using WebMatrix.WebData;

namespace BookWorm.Services.Account
{
    public class AccountService : IAccountService
    {
        public virtual bool Login(string userName, string password, bool persistCookie)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }

        public virtual void Logout()
        {
            WebSecurity.Logout();
        }

        public virtual void CreateUserAndAccount(string userName, string password)
        {
            WebSecurity.CreateUserAndAccount(userName, password);
        }

        public virtual void AddUserToRole(string userName, string role)
        {
            System.Web.Security.Roles.AddUsersToRole(new string[] {userName },role);
        }
    }
}
