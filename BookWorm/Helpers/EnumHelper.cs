using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookWorm.Models;

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