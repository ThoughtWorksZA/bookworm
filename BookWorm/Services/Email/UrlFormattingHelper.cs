using System;

namespace BookWorm.Services.Email
{
    public class UrlFormattingHelper
    {
        public virtual string GetBaseUrl(Uri url)
        {
            return string.Format("{0}://{1}", url.Scheme, url.Authority );
        }
    }
}
