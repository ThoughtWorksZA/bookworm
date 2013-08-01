﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorm.Services.Account
{
    public interface IAccountService
    {
        bool Login(string userName, string password, bool persistCookie);
    }
}
