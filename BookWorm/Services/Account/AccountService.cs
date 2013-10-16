using WebMatrix.WebData;

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
    }
}
