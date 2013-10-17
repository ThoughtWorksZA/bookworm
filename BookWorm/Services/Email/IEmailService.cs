namespace BookWorm.Services.Email
{
    public interface IEmailService
    {
        void SendConfirmation(string from, string to, string securityToken, int userId);
    }
}