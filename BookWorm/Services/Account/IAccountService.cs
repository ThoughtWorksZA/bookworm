namespace BookWorm.Services.Account
{
    public interface IAccountService
    {
        bool Login(string userName, string password, bool persistCookie);
        void Logout();
        void CreateUserAndAccount(string userName, string password);
    }
}
