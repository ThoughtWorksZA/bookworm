namespace BookWorm.Services.Email
{
    public interface IEmail
    {
        void SendConfirmation(string from, string to, string securityToken, int userId);
    }
}