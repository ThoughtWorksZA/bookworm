using WebMatrix.WebData;

namespace BookWorm.Services.Account
{
    public class AccountService:IAccountService
    {
        public bool Login(string userName, string password, bool persistCookie)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }
    }
}