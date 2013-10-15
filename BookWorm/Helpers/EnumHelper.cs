using System;
using System.Linq;
using System.Web.Mvc;

namespace BookWorm.Helpers
{
    public class EnumHelper
    {
        public static SelectList ValidEnumOptions<T>()
        {
            return new SelectList(Enum.GetValues(typeof(T)).Cast<T>().ToList());
        }
    }
}
