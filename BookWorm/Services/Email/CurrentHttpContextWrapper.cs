namespace BookWorm.Services.Email
{
    public class CurrentHttpContextWrapper
    {
        public virtual string GetBaseUrl()
        {
            var urlFormattingHelper = new UrlFormattingHelper();
            return urlFormattingHelper.GetBaseUrl(System.Web.HttpContext.Current.Request.Url);
        }
    }
}